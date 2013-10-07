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
using System.Windows;
using System.Windows.Media;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public class SandPanelWidgetGrid : SandPanel
    {
        //---------------------------------------------------------------------
        #region Fields

        private SandPanelWidgetGridCell[,] _widgetGridCells;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        public static DependencyProperty CellHeightProperty = DependencyProperty.Register( "CellHeight", typeof( double ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( 50.0 ) );
        /// <summary>
        /// Gets or sets the height of a cell.
        /// </summary>
        public double CellHeight
        {
            get { return (double) this.GetValue( SandPanelWidgetGrid.CellHeightProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellHeightProperty, value ); }
        }

        public static DependencyProperty CellPaddingProperty = DependencyProperty.Register( "CellPadding", typeof( Thickness ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Thickness( 10 ) ) );
        /// <summary>
        /// Gets or sets the cell padding.
        /// </summary>
        public Thickness CellPadding
        {
            get { return (Thickness) this.GetValue( SandPanelWidgetGrid.CellPaddingProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellPaddingProperty, value ); }
        }

        public static DependencyProperty CellWidthProperty = DependencyProperty.Register( "CellWidth", typeof( double ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( 50.0 ) );
        /// <summary>
        /// Gets or sets the width of a cell.
        /// </summary>
        public double CellWidth
        {
            get { return (double) this.GetValue( SandPanelWidgetGrid.CellWidthProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellWidthProperty, value ); }
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
            //-- Calculate the total size of a cell
            Size totalCellSize = new Size(

                this.CellWidth + this.CellPadding.Left + this.CellPadding.Right,
                this.CellHeight + this.CellPadding.Top + this.CellPadding.Bottom
            );

            //-- Adjust the panel height and width
            base.Height = this.RowCount * totalCellSize.Height;
            base.Width = this.ColumnCount * totalCellSize.Width;

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
                    cell = new SandPanelWidgetGridCell( totalCellSize, x, y );
                    
                    if( this.ShowGrid )
                    { 
                        cell.BorderBrush = Brushes.Black;
                        cell.BorderThickness = new Thickness( 1 );
                    };

                    _widgetGridCells[x,y] = cell;
                    base.Children.Add( cell );
                    SandPanelWidgetGrid.SetLeft( cell, cellLeft );
                    SandPanelWidgetGrid.SetTop( cell, cellTop );

                    cellLeft += totalCellSize.Width;
                }

                cellTop += totalCellSize.Height;
            }

            #endregion //-- Add all cells

            //-- Call the base implementation
            base.OnInitialized( e );
        }

        protected override void OnItemAdded( SandPanelItem item )
        {
            base.OnItemAdded( item );

            var widget = (SandPanelWidget) item;

            var targetGridCell = this.GetGridCell( widget );

            while( targetGridCell.ContainsWidget )
            {
                //-- Get next grid
                if( targetGridCell.yPosInGrid == this.RowCount - 1 )
                {
                    targetGridCell = _widgetGridCells[targetGridCell.xPosInGrid + 1, 0];
                }
                else
                {
                    targetGridCell = _widgetGridCells[targetGridCell.xPosInGrid, targetGridCell.yPosInGrid + 1];
                }
            }

            widget.HomeWidgetGridCell = targetGridCell;

            targetGridCell.OnWidgetDropped( widget );
        }

        #endregion SandPanel Members
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Calculates the total cell size (including all paddings). 
        /// </summary>
        /// <returns>
        /// The total cell size.
        /// </returns>
        private Size CalculateTotalCellSize()
        {
            return new Size(

                this.CellWidth + this.CellPadding.Left + this.CellPadding.Right,
                this.CellHeight + this.CellPadding.Top + this.CellPadding.Bottom
            );
        }

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
            //-- Calculate the total size of a cell
            Size totalCellSize = this.CalculateTotalCellSize();

            return new Point(

                sandPanelWidgetGridCell.xPosInGrid * totalCellSize.Width,
                sandPanelWidgetGridCell.yPosInGrid * totalCellSize.Height
            );
        }

        private SandPanelWidgetGridCell GetGridCell( SandPanelWidget widget )
        {
            //-- Get the location of the cell within the widget grid
            Point widgetInGridLocation = widget.TranslatePoint( new Point(), this );

            //-- Calculate the total size of a cell
            Size totalCellSize = this.CalculateTotalCellSize();

            //-- Determine the cell where the upper left corner of the widget is located
            int cellXIndex = (int) ( widgetInGridLocation.X / totalCellSize.Width );
            int cellYIndex = (int) ( widgetInGridLocation.Y / totalCellSize.Height );

            //-- Determine the location of the bottom right corner of the cell in order to ...
            Point cellBottomRight = new Point(

                ( cellXIndex + 1 ) * totalCellSize.Width,
                ( cellYIndex + 1 ) * totalCellSize.Height
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

            return _widgetGridCells[cellXIndex, cellYIndex];
        }

        internal void OnWidgetMove( SandPanelWidget widget )
        {
            //-- The mouse is captured in the SandPanelItem.OnMouseDown method
            //-- which will result in a firing of the MouseMove event although
            //-- the MouseDown event handler is not completely processed
            if( !widget.IsMoving ) { return; }

            //-- Determine the affected cell
            var gridCell = this.GetGridCell( widget );

            if( gridCell != widget.HoveredWidgetGridCell )
            {
                widget.HoveredWidgetGridCell.OnWidgetLeave( widget );

                widget.HoveredWidgetGridCell = gridCell;
                gridCell.OnWidgetEnter( widget );
            }
        }

        internal void OnWidgetMovingStarted( SandPanelWidget widget )
        {
            //-- Determine the affected cell
            var gridCell = this.GetGridCell( widget );
                        
            widget.HoveredWidgetGridCell = gridCell;
            gridCell.OnWidgetEnter( widget);
        }

        internal void OnWidgetMovingStopped( SandPanelWidget widget )
        {
            //-- Check preconditions
            if( widget.HoveredWidgetGridCell == null ) { return; }
            
            widget.HoveredWidgetGridCell.OnWidgetDropped( widget );
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------