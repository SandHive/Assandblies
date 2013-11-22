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
    public sealed class SandPanelWidgetGridCellUnion : Border, ISandPanelWidgetGridCell
    {
        //---------------------------------------------------------------------
        #region Fields

        private List<SandPanelWidgetGridCell> _gridCells;

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
        /// Initializes a new instance of the SandPanelWidgetGridCellCollection
        /// class.
        /// </summary>
        /// <param name="capacity">
        /// The collection's capacity.
        /// </param>
        public SandPanelWidgetGridCellUnion( int capacity )
        {
            _gridCells = new List<SandPanelWidgetGridCell>( capacity );
        }

        #endregion Constructors
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

            ////-- Get the location of the cell within the widget grid
            //Point cellInGridLocation = ( (SandPanelWidgetGrid) this.Parent ).GetCellLocation( this );

            ////-- Calculate the offset in order to center the widget
            //double xOffset = ( this.Width - widget.Width ) / 2;
            //double yOffset = ( this.Height - widget.Height ) / 2;

            ////-- Move the widget to the cell's center
            //SandPanelWidgetGrid.SetLeft( widget, cellInGridLocation.X + xOffset );
            //SandPanelWidgetGrid.SetTop( widget, cellInGridLocation.Y + yOffset );

            #endregion //-- Place the widget to the cell's center

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

        public SandPanelWidgetGridCell this[int index]
        {
            get { return _gridCells[index]; }
        }

        public void Add( SandPanelWidgetGridCell cell )
        {
            _gridCells.Add( cell );
        }

        public static bool Equals( SandPanelWidgetGridCellUnion collectionA, SandPanelWidgetGridCellUnion collectionB )
        {
            if( collectionA.Count != collectionB.Count ) { return false; }

            for( int i = 0; i < collectionA.Count; i++ )
            {
                if( !collectionA[i].Guid.Equals( collectionB[i].Guid ) )
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