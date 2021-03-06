﻿/* Copyright (c) 2013 - 2014 The Sandhive Project (http://sandhive.org)
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
using System.Windows.Controls;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    /// <summary>
    /// A union of several SandWidgetGridCell objects.
    /// </summary>
    public sealed class SandWidgetGridCellUnion : SandWidgetGridCellBase
    {
        //---------------------------------------------------------------------
        #region Fields

        private SandWidgetGrid _parentGrid;

        private int _xCellsCount;

        private int _yCellsCount;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandWidgetGridCellUnion
        /// class.
        /// </summary>
        /// <param name="grid">
        /// The parent grid.
        /// </param>
        /// <param name="position">
        /// The position of the cell union within the parent grid.
        /// </param>
        public SandWidgetGridCellUnion( SandWidgetGrid grid, int xCellIndex, int yCellIndex, int xCellsCount, int yCellsCount )
        {
            _parentGrid = grid;
            this.XCellIndex = xCellIndex;
            this.YCellIndex = yCellIndex;
            _xCellsCount = xCellsCount;
            _yCellsCount = yCellsCount;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region SandWidgetGridCellBase Members

        public override bool ContainsWidget
        {
            get
            {
                int bottomRightX = this.XCellIndex + _xCellsCount;
                int bottomRightY = this.YCellIndex + _yCellsCount;
                for (int x = this.XCellIndex; x < bottomRightX; x++)
                {
                    for (int y = this.YCellIndex; y < bottomRightY; y++)
                    {
                        if (_parentGrid.WidgetGridCells[x, y].ContainsWidget)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public override bool IsHome
        {
            get
            {
                return _parentGrid.WidgetGridCells[this.XCellIndex, this.YCellIndex].IsHome;
            }
            internal set
            {
                this.ForEachCellDo( 

                    ( cell ) => cell.IsHome = value
                );
            }
        }

        public override bool IsHovered
        {
            get
            {
                return _parentGrid.WidgetGridCells[this.XCellIndex, this.YCellIndex].IsHovered;
            }
            internal set
            {
                this.ForEachCellDo(

                    ( cell ) => cell.IsHovered = value
                );
            }
        }

        internal override void SetWidget( SandWidgetAdapter widget, bool shouldWidgetBeCentered )
        {
            this.ForEachCellDo( ( cell ) => cell.SetWidget( widget, false ) );

            if( ( widget != null ) && shouldWidgetBeCentered )
            {
                //-- Get the location of the cell within the widget grid
                Point cellInGridLocation = _parentGrid.GetCellLocation( _parentGrid.WidgetGridCells[this.XCellIndex, this.YCellIndex] );

                //-- Calculate the offset in order to center the widget
                double xOffset = ( ( _parentGrid.CellSize.Width * _xCellsCount ) - widget.Width ) / 2;
                double yOffset = ( ( _parentGrid.CellSize.Height * _yCellsCount ) - widget.Height ) / 2;

                //-- Move the widget to the cell's center
                SandWidgetGrid.SetLeft( widget, cellInGridLocation.X + xOffset );
                SandWidgetGrid.SetTop( widget, cellInGridLocation.Y + yOffset );
            }
        }

        internal override SandWidgetAdapter Widget
        {
            get
            {
                return _parentGrid.WidgetGridCells[this.XCellIndex, this.YCellIndex].Widget;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        #endregion SandWidgetGridCellBase Members
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Performs an action on each cell of this union.
        /// </summary>
        /// <param name="action">
        /// The action that should be performed on each cell.
        /// </param>
        private void ForEachCellDo( Action<SandWidgetGridCell> action )
        {
            //-- Call BeginInit / EndInit in oder that the parent grid is updated 
            //-- only one time (and not in every loop run)
            _parentGrid.BeginInit();

            int bottomRightX = this.XCellIndex + _xCellsCount;
            int bottomRightY = this.YCellIndex + _yCellsCount;
            for( int x = this.XCellIndex; x < bottomRightX; x++ )
            {
                for( int y = this.YCellIndex; y < bottomRightY; y++ )
                {
                    action( _parentGrid.WidgetGridCells[x, y] );
                }
            }

            _parentGrid.EndInit();
        }

        /// <summary>
        /// Create some more informative output.
        /// </summary>
        /// <returns>
        /// The string representation of this object.
        /// </returns>
        public override string ToString()
        {
            if( ( _xCellsCount == 1 ) && ( _yCellsCount == 1 ) )
            {
                return String.Format( "({0},{1})", this.XCellIndex, this.YCellIndex );
            }
            else
            {
                return String.Format( "({0},{1}) ({2},{3})", this.XCellIndex, this.YCellIndex, this.XCellIndex + _xCellsCount - 1, this.YCellIndex + _yCellsCount - 1 );
            }
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------