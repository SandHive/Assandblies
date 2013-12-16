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
using System.Windows.Controls;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    /// <summary>
    /// A union of several SandPanelWidgetGridCell objects.
    /// </summary>
    public sealed class SandPanelWidgetGridCellUnion : Border, ISandPanelWidgetGridCell
    {
        //---------------------------------------------------------------------
        #region Fields

        private SandPanelWidgetGrid _parentGrid;

        private int _xCellsCount;

        private int _yCellsCount;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandPanelWidgetGridCellUnion
        /// class.
        /// </summary>
        /// <param name="grid">
        /// The parent grid.
        /// </param>
        /// <param name="position">
        /// The position of the cell union within the parent grid.
        /// </param>
        public SandPanelWidgetGridCellUnion( SandPanelWidgetGrid grid, int xCellIndex, int yCellIndex, int xCellsCount, int yCellsCount )
        {
            _parentGrid = grid;
            this.XCellIndex = xCellIndex;
            this.YCellIndex = yCellIndex;
            _xCellsCount = xCellsCount;
            _yCellsCount = yCellsCount;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region ISandPanelWidgetGridCell Members

        public bool ContainsWidget 
        { 
            get 
            {
                int bottomRightX = this.XCellIndex + _xCellsCount;
                int bottomRightY = this.YCellIndex + _yCellsCount;
                for( int x = this.XCellIndex; x < bottomRightX; x++ )
                {
                    for( int y = this.YCellIndex; y < bottomRightY; y++ )
                    {
                        if( _parentGrid.WidgetGridCells[x, y].ContainsWidget )
                        {
                            return true;
                        }
                    }
                }

                return false;
            } 
        }

        public void OnWidgetDropped( SandPanelWidget widget )
        {
            this.ForEachCellDo( 

                ( cell ) =>
                {
                    cell.Widget = widget;
                    cell.IsWidgetOver = false;
                }
            );
            
            #region //-- Place the widget to the cell's center

            //-- Get the location of the cell within the widget grid
            Point cellInGridLocation = _parentGrid.GetCellLocation( _parentGrid.WidgetGridCells[this.XCellIndex, this.YCellIndex] );

            //-- Calculate the offset in order to center the widget
            double xOffset = ( ( _parentGrid.CellSize.Width * _xCellsCount ) - widget.Width ) / 2;
            double yOffset = ( ( _parentGrid.CellSize.Height * _yCellsCount ) - widget.Height ) / 2;

            //-- Move the widget to the cell's center
            SandPanelWidgetGrid.SetLeft( widget, cellInGridLocation.X + xOffset );
            SandPanelWidgetGrid.SetTop( widget, cellInGridLocation.Y + yOffset );

            #endregion //-- Place the widget to the cell's center

            //-- Update the home cell
            widget.HomeWidgetGridCell = this;
        }

        public void OnWidgetEnter( SandPanelWidget widget )
        {
            this.ForEachCellDo( ( cell ) => cell.OnWidgetEnter( widget ) );
        }

        public void OnWidgetLeave( SandPanelWidget widget )
        {
            this.ForEachCellDo( ( cell ) => cell.OnWidgetLeave( widget ) );
        }

        public SandPanelWidget Widget 
        {
            get
            {
                return _parentGrid.WidgetGridCells[this.XCellIndex, this.YCellIndex].Widget;
            }
            set
            {
                this.ForEachCellDo( ( cell ) => cell.Widget = value );
            }
        }

        public int XCellIndex { get; private set; }

        public int YCellIndex { get; private set; }

        #endregion ISandPanelWidgetGridCell Members
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Performs an action on each cell of this union.
        /// </summary>
        /// <param name="action">
        /// The action that should be performed on each cell.
        /// </param>
        private void ForEachCellDo( Action<SandPanelWidgetGridCell> action )
        {
            int bottomRightX = this.XCellIndex + _xCellsCount;
            int bottomRightY = this.YCellIndex + _yCellsCount;
            for( int x = this.XCellIndex; x < bottomRightX; x++ )
            {
                for( int y = this.YCellIndex; y < bottomRightY; y++ )
                {
                    action( _parentGrid.WidgetGridCells[x, y] );
                }
            }
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------