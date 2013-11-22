/* Copyright (c) 2013 The Sand Hive Project
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
 * IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public class SandPanelWidgetGrid : SandPanel
    {
        //---------------------------------------------------------------------
        #region Fields

        private ISandPanelWidgetGridCell[,] _widgetGridCells;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        public static DependencyProperty CellSizeProperty = DependencyProperty.Register( "CellSize", typeof( Size ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Size( 100.0, 100.0 ) ) );
        /// <summary>
        /// Gets or sets a cell's size.
        /// </summary>
        public Size CellSize
        {
            get { return (Size) this.GetValue( SandPanelWidgetGrid.CellSizeProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellSizeProperty, value ); }
        }

        public static DependencyProperty ColumnCountProperty = DependencyProperty.Register( "ColumnCount", typeof( int ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( 4 ) );
        /// <summary>
        /// Gets or sets the number of columns.
        /// </summary>
        public int ColumnCount
        {
            get { return (int) this.GetValue( SandPanelWidgetGrid.ColumnCountProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.ColumnCountProperty, value ); }
        }

        public static DependencyProperty RowCountProperty = DependencyProperty.Register( "RowCount", typeof( int ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( 4 ) );
        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        public int RowCount
        {
            get { return (int) this.GetValue( SandPanelWidgetGrid.RowCountProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.RowCountProperty, value ); }
        }

        public static DependencyProperty ShowGridProperty = DependencyProperty.Register( "ShowGrid", typeof( bool ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets or sets a flag whether the grid should be shown or not.
        /// </summary>
        public bool ShowGrid
        {
            get { return (bool) this.GetValue( SandPanelWidgetGrid.ShowGridProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.ShowGridProperty, value ); }
        }
        
        #endregion Properties
        //---------------------------------------------------------------------
        #region SandPanel Members

        protected override void OnInitialized( EventArgs e )
        {
            //-- Adjust the panel height and width
            base.Height = this.RowCount * this.CellSize.Height;
            base.Width = this.ColumnCount * this.CellSize.Width;

            #region //-- Add all cells

            SandPanelWidgetGridCell cell;
            double cellLeft = 0;
            double cellTop = 0;
            _widgetGridCells = new SandPanelWidgetGridCell[this.ColumnCount,this.RowCount];
            for( int y = 0; y < this.RowCount; y++ )
            {
                cellLeft = 0;

                for( int x = 0; x < this.ColumnCount; x++ )
                {
                    cell = new SandPanelWidgetGridCell( x, y ) { Height = this.CellSize.Height, Width = this.CellSize.Width };
                    
                    if( this.ShowGrid )
                    { 
                        cell.BorderBrush = Brushes.Black;
                        cell.BorderThickness = new Thickness( 1 );
                    };

                    _widgetGridCells[x,y] = cell;
                    base.Children.Add( cell );
                    SandPanelWidgetGrid.SetLeft( cell, cellLeft );
                    SandPanelWidgetGrid.SetTop( cell, cellTop );

                    cellLeft += this.CellSize.Width;
                }

                cellTop += this.CellSize.Height;
            }

            #endregion //-- Add all cells

            //-- Call the base implementation
            base.OnInitialized( e );
        }

        protected override void OnItemAdded( SandPanelItem item )
        {
            base.OnItemAdded( item );

            var widget = (SandPanelWidget) item;

            //-- Determine the target grid cell (move to the next cell when the current one is occupied)
            var targetGridCell = this.GetOccupiedGridCell( widget );
            while( targetGridCell.ContainsWidget )
            {
                if( targetGridCell.PositionInGrid.LeftTopY == this.RowCount - 1 )
                {
                    //-- Bottom reached. So let's move to the next column's top
                    targetGridCell = _widgetGridCells[targetGridCell.PositionInGrid.LeftTopX + 1, 0];
                }
                else
                {
                    //-- Move just one to the bottom
                    targetGridCell = _widgetGridCells[targetGridCell.PositionInGrid.LeftTopX, targetGridCell.PositionInGrid.LeftTopY + 1];
                }
            }

            //-- Determine the real widget size
            double realWidgetHeight = this.CellSize.Height - ( targetGridCell.Padding.Top + targetGridCell.Padding.Bottom );
            double realWidgetWidth = this.CellSize.Width - ( targetGridCell.Padding.Left + targetGridCell.Padding.Right );
            //-- Adjust the widget size by regarding the tile size
            widget.Height = realWidgetHeight * widget.TileSize.Height;
            widget.Width = realWidgetWidth * widget.TileSize.Width; 

            targetGridCell.OnWidgetDropped( widget );
        }

        #endregion SandPanel Members
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Gets the location of a cell within this grid.
        /// </summary>
        /// <param name="sandPanelWidgetGridCell">
        /// The grid cell whose position should be returned.
        /// </param>
        /// <returns>
        /// The cell's position within the grid.
        /// </returns>
        internal Point GetCellLocation( SandPanelWidgetGridCell sandPanelWidgetGridCell )
        {
            return new Point(

                sandPanelWidgetGridCell.PositionInGrid.LeftTopX * this.CellSize.Width,
                sandPanelWidgetGridCell.PositionInGrid.LeftTopY * this.CellSize.Height
            );
        }

        /// <summary>
        /// Gets the cells that are occupied by the widget.
        /// </summary>
        /// <param name="widget">
        /// The widget.
        /// </param>
        /// <returns>
        /// The cells.
        /// </returns>
        internal ISandPanelWidgetGridCell GetOccupiedGridCell( SandPanelWidget widget )
        {
            //-- Get the location of the cell within the widget grid
            Point widgetInGridLocation = widget.TranslatePoint( new Point(), this );

            //-- Determine the cell where the upper left corner of the widget is located
            int cellXIndex = (int) ( widgetInGridLocation.X / this.CellSize.Width );
            int cellYIndex = (int) ( widgetInGridLocation.Y / this.CellSize.Height );

            //-- Determine the location of the bottom right corner of the cell in order to ...
            Point cellBottomRight = new Point(

                ( cellXIndex + 1 ) * this.CellSize.Width,
                ( cellYIndex + 1 ) * this.CellSize.Height
            );
            //-- ... check if the biggest part of the widget lies in a neighboring cell
            double widgetWidthInCell = cellBottomRight.X - widgetInGridLocation.X;
            double widgetHeightInCell = cellBottomRight.Y - widgetInGridLocation.Y;
            if( widgetWidthInCell < ( widget.ActualWidth / 2 ) ) 
            { 
                cellXIndex++;
            }
            if( widgetHeightInCell < ( widget.ActualHeight / 2 ) )
            {
                cellYIndex++;
            }

            if( ( widget.TileSize.Height > 1 ) || ( widget.TileSize.Width > 1 ) )
            {
                int xOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Width );
                int yOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Height );

                SandPanelWidgetGridCellUnion occupiedCells = new SandPanelWidgetGridCellUnion( xOccupiedCellsCount * yOccupiedCellsCount );

                for( int y = 0; y < yOccupiedCellsCount; y++ )
                {
                    for( int x = 0; x < xOccupiedCellsCount; x++ )
                    {
                        occupiedCells.Add( (SandPanelWidgetGridCell) _widgetGridCells[cellXIndex + x, cellYIndex + y] );
                    }
                }

                return occupiedCells;
            }
            else
            {
                return _widgetGridCells[cellXIndex, cellYIndex];
            }
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------