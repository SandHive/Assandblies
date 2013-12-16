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
using System.Diagnostics;
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

        public static DependencyProperty CellPaddingProperty = DependencyProperty.Register( "CellPadding", typeof( Thickness ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Thickness( 3 ) ) );
        /// <summary>
        /// Gets or sets a cell's padding.
        /// </summary>
        public Thickness CellPadding
        {
            get { return (Thickness) this.GetValue( SandPanelWidgetGrid.CellPaddingProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellPaddingProperty, value ); }
        }

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

        /// <summary>
        /// Gets the collection of widget grid cells.
        /// </summary>
        internal SandPanelWidgetGridCell[,] WidgetGridCells { get { return _widgetGridCells; } }
        
#if PROTOTYPE
        public static DependencyProperty CellIndexesOfBottomRightWidgetCornerProperty = DependencyProperty.Register( "CellIndexesOfBottomRightWidgetCorner", typeof( Point ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Point() ) );
        /// <summary>
        /// Gets the cell indexes of the bottom right widget corner.
        /// </summary>
        public Point CellIndexesOfBottomRightWidgetCorner
        {
            get { return (Point) this.GetValue( SandPanelWidgetGrid.CellIndexesOfBottomRightWidgetCornerProperty ); }
            private set { this.SetValue( SandPanelWidgetGrid.CellIndexesOfBottomRightWidgetCornerProperty, value ); }
        }

        public static DependencyProperty CellIndexesOfTopLeftWidgetCornerProperty = DependencyProperty.Register( "CellIndexesOfTopLeftWidgetCorner", typeof( Point ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Point() ) );
        /// <summary>
        /// Gets the cell indexes of the top left widget corner.
        /// </summary>
        public Point CellIndexesOfTopLeftWidgetCorner
        {
            get { return (Point) this.GetValue( SandPanelWidgetGrid.CellIndexesOfTopLeftWidgetCornerProperty ); }
            private set { this.SetValue( SandPanelWidgetGrid.CellIndexesOfTopLeftWidgetCornerProperty, value ); }
        }

        public static DependencyProperty TopLeftWidgetCornerLocationProperty = DependencyProperty.Register( "TopLeftWidgetCornerLocation", typeof( Point ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Point() ) );
        /// <summary>
        /// Gets the location of the top left widget corner.
        /// </summary>
        public Point TopLeftWidgetCornerLocation
        {
            get { return (Point) this.GetValue( SandPanelWidgetGrid.TopLeftWidgetCornerLocationProperty ); }
            private set { this.SetValue( SandPanelWidgetGrid.TopLeftWidgetCornerLocationProperty, value ); }
        }

        public static DependencyProperty WidgetSizeInBottomRightCellProperty = DependencyProperty.Register( "WidgetSizeInBottomRightCell", typeof( Size ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Size() ) );
        /// <summary>
        /// Gets the widget size that is currently in the bottom right cell.
        /// </summary>
        public Size WidgetSizeInBottomRightCell
        {
            get { return (Size) this.GetValue( SandPanelWidgetGrid.WidgetSizeInBottomRightCellProperty ); }
            private set { this.SetValue( SandPanelWidgetGrid.WidgetSizeInBottomRightCellProperty, value ); }
        }

        public static DependencyProperty WidgetSizeInTopLeftCellProperty = DependencyProperty.Register( "WidgetSizeInTopLeftCell", typeof( Size ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Size() ) );
        /// <summary>
        /// Gets the widget size that is currently in the top left cell.
        /// </summary>
        public Size WidgetSizeInTopLeftCell
        {
            get { return (Size) this.GetValue( SandPanelWidgetGrid.WidgetSizeInTopLeftCellProperty ); }
            private set { this.SetValue( SandPanelWidgetGrid.WidgetSizeInTopLeftCellProperty, value ); }
        }
#endif

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

            //-- Adjust the widget size by regarding the tile size
            widget.Height = ( this.CellSize.Height * widget.TileSize.Height ) - ( this.CellPadding.Top + this.CellPadding.Bottom );
            widget.Width = ( this.CellSize.Width * widget.TileSize.Width ) - ( this.CellPadding.Left + this.CellPadding.Right );

            //-- Determine the number of occupied cells
            int xOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Width );
            int yOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Height );

            //-- Determine the target grid cell (move to the next cell when the current one is occupied)
            var targetGridCell = this.GetNextFreeGridCell( xOccupiedCellsCount, yOccupiedCellsCount );
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

                sandPanelWidgetGridCell.XCellIndex * this.CellSize.Width,
                sandPanelWidgetGridCell.YCellIndex * this.CellSize.Height
            );
        }

        /// <summary>
        /// Gets the next free ISandPanelWidgetGridCell object where the widget will fit into.
        /// </summary>
        /// <param name="xOccupiedCellsCount">
        /// The number of x cells that are required by the widget.
        /// </param>
        /// <param name="yOccupiedCellsCount">
        /// The number of y cells that are required by the widget.
        /// </param>
        /// <exception cref="Exception">
        /// No free fitting grid cell available.
        /// </exception>
        /// <returns>
        /// The next free ISandPanelWidgetGridCell object.
        /// </returns>
        private ISandPanelWidgetGridCell GetNextFreeGridCell( int xOccupiedCellsCount, int yOccupiedCellsCount )
        {
            int xMaxIndex = this.ColumnCount - xOccupiedCellsCount;
            int yMaxIndex = this.RowCount - yOccupiedCellsCount;
            ISandPanelWidgetGridCell nextFreeCell;
            for( int x = 0; x <= xMaxIndex; x++ )
            {
                for( int y = 0; y <= yMaxIndex; y++ )
                {
                    //-- We are searching a ...
                    if( ( xOccupiedCellsCount == 1 ) && ( yOccupiedCellsCount == 1 ) )
                    {
                        //-- ... single free cell
                        nextFreeCell = _widgetGridCells[x, y];
                    }
                    else
                    {
                        //-- ... cell union
                        nextFreeCell = new SandPanelWidgetGridCellUnion( this, x, y, xOccupiedCellsCount, yOccupiedCellsCount );
                    }
 
                    if( !nextFreeCell.ContainsWidget )
                    {
                        return nextFreeCell;
                    }
                }
            }

            throw new Exception( "No free fitting grid cell available!" );
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
            Point topLeftWidgetCornerLocation = widget.TranslatePoint( new Point(), this );

            //-- Determine the cell index where the upper left corner of the widget is located
            int cellXIndexOfTopLeftWidgetCorner = (int) ( topLeftWidgetCornerLocation.X / this.CellSize.Width );
            int cellYIndexOfTopLeftWidgetCorner = (int) ( topLeftWidgetCornerLocation.Y / this.CellSize.Height );

            //-- Determine the number of occupied cells
            int xOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Width );
            int yOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Height );
            
            //-- Determine the cell index where the lower right corner of the widget is located
            int cellXIndexOfBottomRightWidgetCorner = (int) ( ( topLeftWidgetCornerLocation.X + widget.Width ) / this.CellSize.Width );
            int cellYIndexOfBottomRightWidgetCorner = (int) ( ( topLeftWidgetCornerLocation.Y + widget.Height ) / this.CellSize.Height );

            //-- Determine locations for calculating where the biggest part of the widget is located
            Point topLeftCellBottomRightLocation = new Point( ( cellXIndexOfTopLeftWidgetCorner + 1 ) * this.CellSize.Width, ( cellYIndexOfTopLeftWidgetCorner + 1 ) * this.CellSize.Height );
            Point bottomRightCellTopLeftLocation = new Point( ( ( cellXIndexOfBottomRightWidgetCorner ) * this.CellSize.Width ) + 0, ( ( cellYIndexOfBottomRightWidgetCorner ) * this.CellSize.Height ) + 0 );

            //-- Check if the biggest part of the widget lies in a neighboring cell, ...
            double widgetWidthInTopLeftCell = topLeftCellBottomRightLocation.X - topLeftWidgetCornerLocation.X;
            double widgetHeightInTopLeftCell = topLeftCellBottomRightLocation.Y - topLeftWidgetCornerLocation.Y;
            double widgetWidthInBottomRightCell = ( topLeftWidgetCornerLocation.X + widget.Width ) - bottomRightCellTopLeftLocation.X;
            double widgetHeightInBottomRightCell = ( topLeftWidgetCornerLocation.Y + widget.Height ) - bottomRightCellTopLeftLocation.Y;
            
            //-- ... so we have to adjust the indexes
            if( ( widgetWidthInBottomRightCell > widgetWidthInTopLeftCell ) &&
                ( cellXIndexOfTopLeftWidgetCorner <= ( cellXIndexOfBottomRightWidgetCorner - xOccupiedCellsCount ) ) )
            {
                cellXIndexOfTopLeftWidgetCorner++;
            }
            if( ( widgetHeightInBottomRightCell > widgetHeightInTopLeftCell ) &&
                ( cellYIndexOfTopLeftWidgetCorner <= ( cellYIndexOfBottomRightWidgetCorner - yOccupiedCellsCount ) ) )
            {
                cellYIndexOfTopLeftWidgetCorner++;
            }

#if PROTOTYPE
            //-- Assign some values for the prototype UI
            this.CellIndexesOfBottomRightWidgetCorner = new Point( cellXIndexOfBottomRightWidgetCorner, cellYIndexOfBottomRightWidgetCorner );
            this.CellIndexesOfTopLeftWidgetCorner = new Point( cellXIndexOfTopLeftWidgetCorner, cellYIndexOfTopLeftWidgetCorner );
            this.TopLeftWidgetCornerLocation = topLeftWidgetCornerLocation;

            //-- Assign some values for the prototype UI
            this.WidgetSizeInBottomRightCell = new Size( widgetWidthInBottomRightCell, widgetHeightInBottomRightCell );
            this.WidgetSizeInTopLeftCell = new Size( widgetWidthInTopLeftCell, widgetHeightInTopLeftCell );
#endif

            if( ( xOccupiedCellsCount == 1 ) && ( yOccupiedCellsCount == 1 ) )
            {
                //-- There is only a single cell occupied -> return it
                return _widgetGridCells[cellXIndexOfTopLeftWidgetCorner, cellYIndexOfTopLeftWidgetCorner];
            }

            SandPanelWidgetGridCellUnion occupiedCells = new SandPanelWidgetGridCellUnion(

                this,
                cellXIndexOfTopLeftWidgetCorner,
                cellYIndexOfTopLeftWidgetCorner,
                xOccupiedCellsCount,
                yOccupiedCellsCount
            );

            return occupiedCells;
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------
