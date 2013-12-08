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

using System.Collections;
using System.Collections.Generic;
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

        private List<SandPanelWidgetGridCell> _gridCells;

        private int _xOccupiedCellsCount;

        private int _yOccupiedCellsCount;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets the number of grid cells in this collection.
        /// </summary>
        public int Count
        {
            get { return _gridCells.Count; }
        }

        #endregion Properties
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
        public SandPanelWidgetGridCellUnion( SandPanelWidgetGrid grid, SandPanelWidgetGridCellPosition position )
        {
            _parentGrid = grid;
            this.PositionInGrid = position;
            _xOccupiedCellsCount = position.BottomRightX - position.TopLeftX;
            _yOccupiedCellsCount = position.BottomRightY - position.TopLeftY;
            _gridCells = new List<SandPanelWidgetGridCell>( _xOccupiedCellsCount * _yOccupiedCellsCount );
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Indexers & Operators

        public SandPanelWidgetGridCell this[int index]
        {
            get { return _gridCells[index]; }
        }

        #endregion Indexers & Operators
        //---------------------------------------------------------------------
        #region ISandPanelWidgetGridCell Members

        public bool ContainsWidget 
        { 
            get 
            {
                foreach( var cell in _gridCells )
                {
                    if( cell.ContainsWidget )
                    {
                        return true;
                    }
                }

                return false;
            } 
        }

        public void OnWidgetDropped( SandPanelWidget widget )
        {
            foreach( var cell in _gridCells )
            {
                cell.Widget = widget;
                cell.IsWidgetOver = false;
            }
            
            #region //-- Place the widget to the cell's center

            //-- Get the location of the cell within the widget grid
            Point cellInGridLocation = _parentGrid.GetCellLocation( _gridCells[0] );

            //-- Calculate the offset in order to center the widget
            double xOffset = ( ( _parentGrid.CellSize.Width * _xOccupiedCellsCount ) - widget.Width ) / 2;
            double yOffset = ( ( _parentGrid.CellSize.Height * _yOccupiedCellsCount ) - widget.Height ) / 2;

            //-- Move the widget to the cell's center
            SandPanelWidgetGrid.SetLeft( widget, cellInGridLocation.X + xOffset );
            SandPanelWidgetGrid.SetTop( widget, cellInGridLocation.Y + yOffset );

            #endregion //-- Place the widget to the cell's center

            //-- Update the home cell
            widget.HomeWidgetGridCell = this;
        }

        public void OnWidgetEnter( SandPanelWidget widget )
        {
            foreach( var cell in _gridCells )
            {
                cell.OnWidgetEnter( widget );
            }
        }

        public void OnWidgetLeave( SandPanelWidget widget )
        {
            foreach( var cell in _gridCells )
            {
                cell.OnWidgetLeave( widget );
            }
        }

        public SandPanelWidgetGridCellPosition PositionInGrid { get; private set; }

        public SandPanelWidget Widget 
        {
            get
            {
                if( ( _gridCells != null ) && ( _gridCells.Count > 0 ) )
                {
                    return _gridCells[0].Widget;
                }

                return null;
            }
            set
            {
                foreach( var cell in _gridCells )
                {
                    cell.Widget = value;
                }
            }
        }

        #endregion ISandPanelWidgetGridCell Members
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Adds a grid cell to this union.
        /// </summary>
        /// <param name="cell">
        /// The SandPanelWidgetGridCell object.
        /// </param>
        public void Add( SandPanelWidgetGridCell cell )
        {
            _gridCells.Add( cell );
        }

        /// <summary>
        /// Checks if two unions are equal.
        /// </summary>
        /// <param name="unionA">
        /// Union A.
        /// </param>
        /// <param name="unionB">
        /// Union B.
        /// </param>
        /// <returns>
        /// True = equal,
        /// False = different.
        /// </returns>
        public static bool Equals( SandPanelWidgetGridCellUnion unionA, SandPanelWidgetGridCellUnion unionB )
        {
            if( unionA.Count != unionB.Count ) { return false; }

            for( int i = 0; i < unionA.Count; i++ )
            {
                if( !unionA[i].Guid.Equals( unionB[i].Guid ) )
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------