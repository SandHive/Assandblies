/* Copyright (c) 2013 - 2014 The Sand Hive Project
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
    public class SandWidgetGrid : SandPanel
    {
        //---------------------------------------------------------------------
        #region Fields
        
        private int _beginInitCounter = 0;

        private readonly Guid _guid = Guid.NewGuid();

        private SandWidgetGridCell[,] _widgetGridCells;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        public static DependencyProperty CellPaddingProperty = DependencyProperty.Register( "CellPadding", typeof( Thickness ), typeof( SandWidgetGrid ), new PropertyMetadata( new Thickness( 3 ) ) );
        /// <summary>
        /// Gets or sets a cell's padding.
        /// </summary>
        public Thickness CellPadding
        {
            get { return (Thickness) this.GetValue( SandWidgetGrid.CellPaddingProperty ); }
            set { this.SetValue( SandWidgetGrid.CellPaddingProperty, value ); }
        }

        public static DependencyProperty CellSizeProperty = DependencyProperty.Register( "CellSize", typeof( Size ), typeof( SandWidgetGrid ), new PropertyMetadata( new Size( 100.0, 100.0 ) ) );
        /// <summary>
        /// Gets or sets a cell's size.
        /// </summary>
        public Size CellSize
        {
            get { return (Size) this.GetValue( SandWidgetGrid.CellSizeProperty ); }
            set { this.SetValue( SandWidgetGrid.CellSizeProperty, value ); }
        }

        public static DependencyProperty ColumnCountProperty = DependencyProperty.Register( "ColumnCount", typeof( int ), typeof( SandWidgetGrid ), new PropertyMetadata( 4 ) );
        /// <summary>
        /// Gets or sets the number of columns.
        /// </summary>
        public int ColumnCount
        {
            get { return (int) this.GetValue( SandWidgetGrid.ColumnCountProperty ); }
            set { this.SetValue( SandWidgetGrid.ColumnCountProperty, value ); }
        }

        /// <summary>
        /// Gets a unique identifier for this object.
        /// </summary>
        public Guid Guid { get { return _guid; } }

        public static DependencyProperty RowCountProperty = DependencyProperty.Register( "RowCount", typeof( int ), typeof( SandWidgetGrid ), new PropertyMetadata( 4 ) );
        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        public int RowCount
        {
            get { return (int) this.GetValue( SandWidgetGrid.RowCountProperty ); }
            set { this.SetValue( SandWidgetGrid.RowCountProperty, value ); }
        }

        public static DependencyProperty ShowGridProperty = DependencyProperty.Register( "ShowGrid", typeof( bool ), typeof( SandWidgetGrid ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets or sets a flag whether the grid should be shown or not.
        /// </summary>
        public bool ShowGrid
        {
            get { return (bool) this.GetValue( SandWidgetGrid.ShowGridProperty ); }
            set { this.SetValue( SandWidgetGrid.ShowGridProperty, value ); }
        }

        /// <summary>
        /// Gets the collection of widget grid cells.
        /// </summary>
        internal SandWidgetGridCell[,] WidgetGridCells { get { return _widgetGridCells; } }
        
#if PROTOTYPE
        public static DependencyProperty CellIndexesOfBottomRightWidgetCornerProperty = DependencyProperty.Register( "CellIndexesOfBottomRightWidgetCorner", typeof( Point ), typeof( SandWidgetGrid ), new PropertyMetadata( new Point() ) );
        /// <summary>
        /// Gets the cell indexes of the bottom right widget corner.
        /// </summary>
        public Point CellIndexesOfBottomRightWidgetCorner
        {
            get { return (Point) this.GetValue( SandWidgetGrid.CellIndexesOfBottomRightWidgetCornerProperty ); }
            private set { this.SetValue( SandWidgetGrid.CellIndexesOfBottomRightWidgetCornerProperty, value ); }
        }

        public static DependencyProperty CellIndexesOfTopLeftWidgetCornerProperty = DependencyProperty.Register( "CellIndexesOfTopLeftWidgetCorner", typeof( Point ), typeof( SandWidgetGrid ), new PropertyMetadata( new Point() ) );
        /// <summary>
        /// Gets the cell indexes of the top left widget corner.
        /// </summary>
        public Point CellIndexesOfTopLeftWidgetCorner
        {
            get { return (Point) this.GetValue( SandWidgetGrid.CellIndexesOfTopLeftWidgetCornerProperty ); }
            private set { this.SetValue( SandWidgetGrid.CellIndexesOfTopLeftWidgetCornerProperty, value ); }
        }

        public static DependencyProperty TopLeftWidgetCornerLocationProperty = DependencyProperty.Register( "TopLeftWidgetCornerLocation", typeof( Point ), typeof( SandWidgetGrid ), new PropertyMetadata( new Point() ) );
        /// <summary>
        /// Gets the location of the top left widget corner.
        /// </summary>
        public Point TopLeftWidgetCornerLocation
        {
            get { return (Point) this.GetValue( SandWidgetGrid.TopLeftWidgetCornerLocationProperty ); }
            private set { this.SetValue( SandWidgetGrid.TopLeftWidgetCornerLocationProperty, value ); }
        }

        public static DependencyProperty WidgetSizeInBottomRightCellProperty = DependencyProperty.Register( "WidgetSizeInBottomRightCell", typeof( Size ), typeof( SandWidgetGrid ), new PropertyMetadata( new Size() ) );
        /// <summary>
        /// Gets the widget size that is currently in the bottom right cell.
        /// </summary>
        public Size WidgetSizeInBottomRightCell
        {
            get { return (Size) this.GetValue( SandWidgetGrid.WidgetSizeInBottomRightCellProperty ); }
            private set { this.SetValue( SandWidgetGrid.WidgetSizeInBottomRightCellProperty, value ); }
        }

        public static DependencyProperty WidgetSizeInTopLeftCellProperty = DependencyProperty.Register( "WidgetSizeInTopLeftCell", typeof( Size ), typeof( SandWidgetGrid ), new PropertyMetadata( new Size() ) );
        /// <summary>
        /// Gets the widget size that is currently in the top left cell.
        /// </summary>
        public Size WidgetSizeInTopLeftCell
        {
            get { return (Size) this.GetValue( SandWidgetGrid.WidgetSizeInTopLeftCellProperty ); }
            private set { this.SetValue( SandWidgetGrid.WidgetSizeInTopLeftCellProperty, value ); }
        }
#endif

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandWidgetGrid class.
        /// </summary>
        public SandWidgetGrid()
        {
            //-- Register this grid to the widget positioner in order to enable
            //-- widget movements across different grids
            SandWidgetPositioner.RegisterGrid( this );
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region SandPanel Members

        public override void AddItem( SandPanelItem item )
        {
            throw new NotSupportedException( "Only \"SandWidget\" objects can be added! Use \"AddWidget\" methods instead." );
        }

        public override void BeginInit()
        {
            //-- Take care that BeginInit and EndInit is called only once
            if( _beginInitCounter == 0 )
            {
                base.BeginInit();
            }
            _beginInitCounter++;
        }

        public override void EndInit()
        {
            //-- Take care that BeginInit and EndInit is called only once
            if( _beginInitCounter == 1 )
            {
                base.EndInit();
            }
            _beginInitCounter--;
        }

        protected override void OnInitialized( EventArgs e )
        {
            //-- Adjust the panel height and width
            base.Height = this.RowCount * this.CellSize.Height;
            base.Width = this.ColumnCount * this.CellSize.Width;

            #region //-- Add all cells

            SandWidgetGridCell cell;
            double cellLeft = 0;
            double cellTop = 0;
            _widgetGridCells = new SandWidgetGridCell[this.ColumnCount,this.RowCount];
            for( int y = 0; y < this.RowCount; y++ )
            {
                cellLeft = 0;

                for( int x = 0; x < this.ColumnCount; x++ )
                {
                    cell = new SandWidgetGridCell( x, y ) { Height = this.CellSize.Height, Width = this.CellSize.Width };
                    
                    if( this.ShowGrid )
                    { 
                        cell.BorderBrush = Brushes.Black;
                        cell.BorderThickness = new Thickness( 1 );
                    };

                    _widgetGridCells[x,y] = cell;
                    base.Children.Add( cell );
                    SandWidgetGrid.SetLeft( cell, cellLeft );
                    SandWidgetGrid.SetTop( cell, cellTop );

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
            //-- This method is called when a widget was added via xaml

            if( !( item is SandWidget ) )
            {
                throw new ArgumentException( "Only items of type \"SandWidget\" can be added!" );
            }

            base.OnItemAdded( item );

            var widget = (SandWidget) item;
            this.CalculateWidgetWidthAndHeight( widget );
            var targetGridCell = this.GetNextFreeGridCell( widget );
            targetGridCell.OnWidgetDropped( widget );
        }

        #endregion SandPanel Members
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Adds a widget into the next free cell of the widget grid.
        /// </summary>
        /// <param name="widget">
        /// The widget that should be added.
        /// </param>
        public void AddWidget( SandWidget widget )
        {
            base.Children.Add( widget );
            base.OnItemAdded( widget );

            this.CalculateWidgetWidthAndHeight( widget );
            var targetGridCell = this.GetNextFreeGridCell( widget );
            targetGridCell.OnWidgetDropped( widget );
        }

        /// <summary>
        /// Adds a widget into a desired cell of the widget grid.
        /// </summary>
        /// <param name="widget">
        /// The widget that should be added.
        /// </param>
        /// <param name="xCellIndex">
        /// The x index of the cell into which the widget should be added.
        /// </param>
        /// <param name="yCellIndex">
        /// The x index of the cell into which the widget should be added.
        /// </param>
        public void AddWidget( SandWidget widget, int xCellIndex, int yCellIndex )
        {
            base.Children.Add( widget );
            base.OnItemAdded( widget );

            this.CalculateWidgetWidthAndHeight( widget );
            var targetGridCell = _widgetGridCells[xCellIndex, yCellIndex];
            targetGridCell.OnWidgetDropped( widget );
        }

        /// <summary>
        /// Calculates the widget width and height by considering the widget's 
        /// tile size and the cell padding.
        /// </summary>
        /// <param name="widget">
        /// The SandWidget object whose width and height should be calculated.
        /// </param>
        private void CalculateWidgetWidthAndHeight( SandWidget widget )
        {
            //-- Adjust the widget size by regarding the tile size
            widget.Height = ( this.CellSize.Height * widget.TileSize.Height ) - ( this.CellPadding.Top + this.CellPadding.Bottom );
            widget.Width = ( this.CellSize.Width * widget.TileSize.Width ) - ( this.CellPadding.Left + this.CellPadding.Right );
        }

        /// <summary>
        /// Gets a cell relative to a given point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <returns>
        /// The related cell.
        /// </returns>
        public SandWidgetGridCell GetCellRelativeToPoint( Point point )
        {
            //-- Determine indexes
            int cellXIndex = (int) ( point.X / this.CellSize.Width );
            int cellYIndex = (int) ( point.Y / this.CellSize.Height );

            //-- Adjust the x index
            if( cellXIndex >= this.ColumnCount ) { cellXIndex = this.ColumnCount - 1; }
            else if( cellXIndex < 0 ) { cellXIndex = 0; }
            //-- Adjust the y index
            if( cellYIndex >= this.RowCount ) { cellYIndex = this.RowCount - 1; }
            else if( cellYIndex < 0 ) { cellYIndex = 0; }

            return _widgetGridCells[cellXIndex, cellYIndex];
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
        internal Point GetCellLocation( SandWidgetGridCell sandPanelWidgetGridCell )
        {
            return new Point(

                sandPanelWidgetGridCell.XCellIndex * this.CellSize.Width,
                sandPanelWidgetGridCell.YCellIndex * this.CellSize.Height
            );
        }

        /// <summary>
        /// Gets the next free ISandWidgetGridCell object where the widget will fit into.
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
        /// The next free ISandWidgetGridCell object.
        /// </returns>
        private ISandWidgetGridCell GetNextFreeGridCell( SandWidget widget )
        {
            //-- Determine the number of occupied cells
            int xOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Width );
            int yOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Height );

            int xMaxIndex = this.ColumnCount - xOccupiedCellsCount;
            int yMaxIndex = this.RowCount - yOccupiedCellsCount;
            ISandWidgetGridCell nextFreeCell;
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
                        nextFreeCell = new SandWidgetGridCellUnion( this, x, y, xOccupiedCellsCount, yOccupiedCellsCount );
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
        internal ISandWidgetGridCell GetOccupiedGridCell( SandWidget widget )
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

            SandWidgetGridCellUnion occupiedCells = new SandWidgetGridCellUnion(

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
