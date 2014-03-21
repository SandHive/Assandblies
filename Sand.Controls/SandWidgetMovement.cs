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
using System.Collections.Generic;
using System.Diagnostics;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    /// <summary>
    /// Contains all data about a moving widget (e.g. from where it starts,
    /// which other widgets had to be moved, and so on ...).
    /// </summary>
    internal sealed class SandWidgetMovement
    {
        //---------------------------------------------------------------------
        #region Fields

        private ISandWidgetGridCell _currentCell;

        private ISandWidgetGridCell _homeCell;

        private Dictionary<Guid, SandWidgetMovement>  _subMovements;

        private SandWidget _widget;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandWidgetMovement class.
        /// </summary>
        /// <param name="widget">
        /// The widget that is moving.
        /// </param>
        /// <param name="homeCell">
        /// The home cell of the moving widget.
        /// </param>
        private SandWidgetMovement( SandWidget widget, ISandWidgetGridCell homeCell )
        {
            _widget = widget;
            _homeCell = homeCell;
            _currentCell = homeCell;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Methods

        internal void MoveWidgetTo( ISandWidgetGridCell newCurrentCell )
        {
            if( newCurrentCell == null )
            {
                throw new ArgumentNullException( "The new target cell may not be null!" );
            }
            if( newCurrentCell == _currentCell ) { return; }

            var grid = (SandWidgetGrid) _widget.Parent;

            grid.BeginInit();

            //-- Remove the widget from the current cell
            _currentCell.OnWidgetLeave( _widget );

            //-- Check if the new current cell contains a widget that has to be moved to the old current cell
            if( newCurrentCell.ContainsWidget )
            {
                //-- Keep the widget that should be swapped in mind
                var swapWidget = newCurrentCell.Widget;

                //-- Remove the swap widget from the new current cell
                newCurrentCell.Widget = null;

                //-- Add the swap widget to the old current cell
                _currentCell.OnWidgetDropped( swapWidget );

                if( _subMovements == null )
                {
                    _subMovements = new Dictionary<Guid, SandWidgetMovement>();
                }

                foreach( var subMovement in _subMovements.Values )
                {
                    this.ValidateSubMovement( subMovement );
                }

                if( !_subMovements.ContainsKey( swapWidget.Guid ) )
                {
                    _subMovements.Add( swapWidget.Guid, new SandWidgetMovement( swapWidget, newCurrentCell ) );

                    Debug.WriteLine( string.Format( "# sub movements: {0}", _subMovements.Count ) );
                }
            }
            
            //-- Add the moving widget to the new current cell ...
            _currentCell = newCurrentCell;
            //-- ... (but do not use the "OnWidgetDropped" method to avoid a 
            //-- flickering when the widget is centered within the cell)
            _currentCell.Widget = _widget;
            _currentCell.OnWidgetEnter( _widget );

            grid.EndInit();
        }

        internal static SandWidgetMovement Start( SandWidget widget, ISandWidgetGridCell homeCell )
        {
            homeCell.IsHome = true;
            homeCell.OnWidgetEnter( widget );

            return new SandWidgetMovement( widget, homeCell );
        }

        internal void Stop()
        {
            Debug.WriteLine( string.Format( "Widget moving stopped (Name: {0}; Cell: {1})", _widget.Name, _currentCell ) );

            //-- 
            _homeCell.IsHome = false;
            _currentCell.OnWidgetDropped( _widget );
        }

        public override string ToString()
        {
            //-- Assemble the main movement data
            string result = String.Format( "( Widget Name: {0}, Home Cell: {1}, Current Cell: {2} )", _widget.Name, _homeCell, _currentCell );

            #region //-- Assemble the sub movement data

            //if( ( this.SubMovements != null ) && ( this.SubMovements.Count > 0 ) )
            //{
            //    result += String.Format( "\r\n\t=== Sub Movements ({0}) ===", this.SubMovements.Count );

            //    foreach( var subMovement in this.SubMovements )
            //    {
            //        result += String.Format( "\r\n\t- {0}", subMovement );
            //    }
            //}

            #endregion //-- Assemble the sub movement data

            return result;
        }

        private void ValidateSubMovement( SandWidgetMovement subMovement )
        {
            if( subMovement._homeCell == _currentCell ) { return; }

            //--

        }

        #endregion Methods
        //---------------------------------------------------------------------

    }
}
//-----------------------------------------------------------------------------