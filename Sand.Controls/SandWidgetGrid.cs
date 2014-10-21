/* Copyright (c) 2013 - 2014 The Sandhive Project (http://sandhive.org)
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

using Sand.Controls.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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

        private List<SandWidgetAdapter> _widgetAdapters = new List<SandWidgetAdapter>();

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
            _widgetGridCells = new SandWidgetGridCell[this.ColumnCount, this.RowCount];
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

                    _widgetGridCells[x, y] = cell;
                    base.Children.Add( cell );
                    SandWidgetGrid.SetLeft( cell, cellLeft );
                    SandWidgetGrid.SetTop( cell, cellTop );

                    cellLeft += this.CellSize.Width;
                }

                cellTop += this.CellSize.Height;
            }

            #endregion //-- Add all cells

            //-- Get all via xaml added ISandWidget objects
            List<UIElement> ISandWidgetObjects = new List<UIElement>();
            foreach( UIElement child in this.Children )
            {
                if( child is ISandWidget )
                {
                    ISandWidgetObjects.Add( child );
                }
            }

            //-- Remove each ISandWidget objects from the Children collection, wrap it and add the wrapper instead
            foreach( UIElement childToBeWrapped in ISandWidgetObjects )
            {
                this.Children.Remove( childToBeWrapped );
                this.Children.Add( new SandWidgetAdapter( (ISandWidget) childToBeWrapped ) );
            }

            //-- Call the base implementation
            base.OnInitialized( e );
            
            //-- Just register this grid instance to the grid manager
            SandWidgetGridsManager.RegisterGrid( this );
        }

        protected override void OnItemAdded( SandPanelItem item )
        {
            //-- This method is called when a widget was added via xaml

            if( !( item is SandWidgetAdapter ) )
            {
                throw new ArgumentException( "Only items of type \"SandWidgetAdapter\" can be added!" );
            }

            //-- Get the widget wrapper
            var widgetAdapter = (SandWidgetAdapter) item;
                    
            //-- The widget should not be added to the children collection,
            //-- because this was already done deep in the xaml world
            this.AddWidget( widgetAdapter, -1, -1, false );
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
        [DebuggerStepThrough]
        public void AddWidget( ISandWidget widget )
        {
            this.AddWidget( widget, -1, -1 );
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
        [DebuggerStepThrough]
        public void AddWidget( ISandWidget widget, int xCellIndex, int yCellIndex )
        {
            //-- Create an adapter for the ISandWidget object in order to equip it with widget functionality
            var widgetAdapter = new SandWidgetAdapter( widget );

            this.AddWidget( widgetAdapter, xCellIndex, yCellIndex, true );
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
        /// <param name="shouldWidgetBeAddedToChildrenCollection">
        /// When set to 'true', the widget will be added to the children 
        /// collection.
        /// </param>
        private void AddWidget( SandWidgetAdapter widgetAdapter, int xCellIndex, int yCellIndex, bool shouldWidgetBeAddedToChildrenCollection )
        {
            //-- Store the widget adapter
            _widgetAdapters.Add( widgetAdapter );

            //-- When a widget is added via xaml it is already in the Children collection
            if( shouldWidgetBeAddedToChildrenCollection )
            {
                base.Children.Add( widgetAdapter );
            }

            //-- Call the base implementation
            base.OnItemAdded( widgetAdapter );
            
            //-- Adjust the widget size by regarding the tile size
            widgetAdapter.Height = ( this.CellSize.Height * widgetAdapter.Widget.TileSize.Height ) - ( this.CellPadding.Top + this.CellPadding.Bottom );
            widgetAdapter.Width = ( this.CellSize.Width * widgetAdapter.Widget.TileSize.Width ) - ( this.CellPadding.Left + this.CellPadding.Right );

            //-- Determine the target cell
            SandWidgetGridCellBase targetCell;
            if( ( xCellIndex > -1 ) && ( yCellIndex > -1 ) )
            {
                targetCell = _widgetGridCells[xCellIndex, yCellIndex];
            }
            else
            {
                targetCell = this.GetNextFreeGridCell( widgetAdapter.Widget );
            }

            //-- Set the widget to the target cell ...
            targetCell.SetWidget( widgetAdapter );

            //-- ... and make the target cell the home cell of the widget
            widgetAdapter.HomeCell = targetCell;
        }

        /// <summary>
        /// Gets the current widget allocation of this grid.
        /// </summary>
        /// <returns>
        /// The current widget allocation of this grid.
        /// </returns>
        public SandWidgetGridAllocation GetAllocation()
        {
            List<SandWidgetAllocationInfo> widgetsAllocationInfo = new List<SandWidgetAllocationInfo>( _widgetAdapters.Count );
            foreach( var widgetAdapter in _widgetAdapters )
            {
                widgetsAllocationInfo.Add( 
                    
                    new SandWidgetAllocationInfo( 
                        
                        widgetAdapter.Widget.WidgetFrameworkElement.Name, 
                        new Point( widgetAdapter.HomeCell.XCellIndex, widgetAdapter.HomeCell.YCellIndex ),
                        widgetAdapter.Widget.TileSize
                    )
                );
            }

            return SandWidgetGridAllocation.Create( this.ColumnCount, this.RowCount, widgetsAllocationInfo );
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
        /// <param name="sandWidgetGridCell">
        /// The grid cell whose position should be returned.
        /// </param>
        /// <returns>
        /// The cell's position within the grid.
        /// </returns>
        internal Point GetCellLocation( SandWidgetGridCell sandWidgetGridCell )
        {
            return new Point(

                sandWidgetGridCell.XCellIndex * this.CellSize.Width,
                sandWidgetGridCell.YCellIndex * this.CellSize.Height
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
        private SandWidgetGridCellBase GetNextFreeGridCell( ISandWidget widget )
        {
            //-- Determine the number of occupied cells
            int xOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Width );
            int yOccupiedCellsCount = (int) Math.Ceiling( widget.TileSize.Height );

            int xMaxIndex = this.ColumnCount - xOccupiedCellsCount;
            int yMaxIndex = this.RowCount - yOccupiedCellsCount;
            SandWidgetGridCellBase nextFreeCell;
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
        /// <param name="widgetAdapter">
        /// The widget.
        /// </param>
        /// <returns>
        /// The cells.
        /// </returns>
        internal SandWidgetGridCellBase GetOccupiedGridCell( SandWidgetAdapter widgetAdapter )
        {
            //-- Get the location of the cell within the widget grid
            Point topLeftWidgetCornerLocation = widgetAdapter.TranslatePoint( new Point(), this );

            //-- Determine the cell index where the upper left corner of the widget is located
            int cellXIndexOfTopLeftWidgetCorner = (int) ( topLeftWidgetCornerLocation.X / this.CellSize.Width );
            int cellYIndexOfTopLeftWidgetCorner = (int) ( topLeftWidgetCornerLocation.Y / this.CellSize.Height );

            //-- Determine the number of occupied cells
            int xOccupiedCellsCount = (int) Math.Ceiling( widgetAdapter.Widget.TileSize.Width );
            int yOccupiedCellsCount = (int) Math.Ceiling( widgetAdapter.Widget.TileSize.Height );

            //-- Determine the cell index where the lower right corner of the widget is located
            int cellXIndexOfBottomRightWidgetCorner = (int) ( ( topLeftWidgetCornerLocation.X + widgetAdapter.Width ) / this.CellSize.Width );
            int cellYIndexOfBottomRightWidgetCorner = (int) ( ( topLeftWidgetCornerLocation.Y + widgetAdapter.Height ) / this.CellSize.Height );

            //-- Determine locations for calculating where the biggest part of the widget is located
            Point topLeftCellBottomRightLocation = new Point( ( cellXIndexOfTopLeftWidgetCorner + 1 ) * this.CellSize.Width, ( cellYIndexOfTopLeftWidgetCorner + 1 ) * this.CellSize.Height );
            Point bottomRightCellTopLeftLocation = new Point( ( ( cellXIndexOfBottomRightWidgetCorner ) * this.CellSize.Width ) + 0, ( ( cellYIndexOfBottomRightWidgetCorner ) * this.CellSize.Height ) + 0 );

            //-- Check if the biggest part of the widget lies in a neighboring cell, ...
            double widgetWidthInTopLeftCell = topLeftCellBottomRightLocation.X - topLeftWidgetCornerLocation.X;
            double widgetHeightInTopLeftCell = topLeftCellBottomRightLocation.Y - topLeftWidgetCornerLocation.Y;
            double widgetWidthInBottomRightCell = ( topLeftWidgetCornerLocation.X + widgetAdapter.Width ) - bottomRightCellTopLeftLocation.X;
            double widgetHeightInBottomRightCell = ( topLeftWidgetCornerLocation.Y + widgetAdapter.Height ) - bottomRightCellTopLeftLocation.Y;

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

		public bool LoadAllocation()
		{
			var settings = new Settings();

			if( !String.IsNullOrEmpty( settings.GridAllocation ) )
			{
				var blubb = Convert.FromBase64String( settings.GridAllocation );

				using( MemoryStream memStream = new MemoryStream( blubb ) )
				{
					BinaryFormatter serializer = new BinaryFormatter();
					var gridAllocation = (SandWidgetGridAllocation) serializer.Deserialize( memStream );

					
					//foreach( var widgetAdapter in _widgetAdapters )
					//{
					//	this.Children.Remove( widgetAdapter );
					//}
					//_widgetAdapters.Clear();

					foreach( var widgetAllocationInfo in gridAllocation.widgetsAllocationInfo )
					{
						//this.AddWidget( new ImageW)
					}
				}

				return true;
			}

			return false;
		}

		public void SaveAllocation()
		{
			var gridAllocation = this.GetAllocation();

			using( MemoryStream memStream = new MemoryStream() )
			{
				BinaryFormatter serializer = new BinaryFormatter();
				serializer.Serialize( memStream, gridAllocation );

				var settings = new Settings();
				settings.GridAllocation = Convert.ToBase64String( memStream.ToArray() );

				settings.Save();
			}
		}

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------
