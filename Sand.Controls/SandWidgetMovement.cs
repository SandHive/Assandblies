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

        private Dictionary<Guid, SandWidgetMovement>  _subMovements;

        private SandWidgetWrapper _widget;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Property

        /// <summary>
        /// Gets the current cell of the moving widegt.
        /// </summary>
        internal SandWidgetGridCellBase CurrentCell { get; private set; }

        /// <summary>
        /// Gets the home cell of the current moving widget.
        /// </summary>
        internal SandWidgetGridCellBase HomeCell { get; private set; }

        #endregion Property
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
        private SandWidgetMovement( SandWidgetWrapper widget, SandWidgetGridCellBase homeCell )
        {
            _widget = widget;
            this.HomeCell = homeCell;
            this.CurrentCell = homeCell;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Methods

        [DebuggerStepThrough]
        internal void MoveWidgetTo( SandWidgetGridCellBase newCurrentCell )
        {
            this.MoveWidgetTo( newCurrentCell, false );
        }

        internal void MoveWidgetTo( SandWidgetGridCellBase newCurrentCell, bool shouldWidgetBeCentered )
        {
            if( newCurrentCell == null )
                throw new ArgumentNullException( "The new target cell may not be null!" );
            
            if( newCurrentCell == this.CurrentCell ) 
                return;

            var grid = (SandWidgetGrid) _widget.Parent;

            grid.BeginInit();

            //-- Remove the widget from the current cell
            this.CurrentCell.SetWidget( null );
            this.CurrentCell.IsHovered = false;

            //-- Check if the new current cell contains a widget that has to be moved to the old current cell
            if( newCurrentCell.ContainsWidget )
            {
                //-- Keep the widget that should be swapped in mind
                var swapWidget = newCurrentCell.Widget;

                //-- Add the swap widget to the old current cell
                this.CurrentCell.SetWidget( swapWidget ); 

                if( _subMovements == null )
                {
                    _subMovements = new Dictionary<Guid, SandWidgetMovement>();
                }

                if( !_subMovements.ContainsKey( swapWidget.Guid ) )
                {
                    var movement = new SandWidgetMovement( swapWidget, newCurrentCell );
                    //movement.MoveWidgetTo( _currentCell );
                    _subMovements.Add( swapWidget.Guid, movement );

                    Debug.WriteLine( string.Format( "# sub movements: {0}", _subMovements.Count ) );
                }
            }
            
            //-- Add the moving widget to the new current cell
            this.CurrentCell = newCurrentCell;
            this.CurrentCell.SetWidget( _widget, shouldWidgetBeCentered );
            this.CurrentCell.IsHovered = true;


            #region //-- Validate sub movements

            if( _subMovements != null )
            {
                //-- Check all swapped widgets if their own home cell is not occupied by the moving 
                //-- widget any more. In this case replace the swapped widget with the one in its 
                //-- home cell.
                foreach( var subMovement in _subMovements.Values )
                {
                    //-- Continue when the moving widget's current cell is the home cell of the swapped widget.
                    if( subMovement.HomeCell == this.CurrentCell )
                        continue;

                    var blubbWidget = subMovement.HomeCell.Widget;
                    //subMovement.CurrentCell.Widget = blubbWidget;
                    //subMovement.HomeCell.Widget = subMovement._widget;
                }
            }

            #endregion //-- Validate sub movements


            grid.EndInit();
        }

        internal static SandWidgetMovement Start( SandWidgetWrapper widget, SandWidgetGridCellBase homeCell )
        {
            //-- Check arguments
            if( widget == null )
                throw new ArgumentNullException( null, "The widget may not be null!" );
            if( homeCell == null )
                throw new ArgumentNullException( null, "The home cell may not be null!" );

            homeCell.IsHome = true;

            return new SandWidgetMovement( widget, homeCell );
        }

        internal void Stop()
        {
            Debug.WriteLine( string.Format( "Widget moving stopped (Name: {0}; Cell: {1})", _widget.Name, this.CurrentCell ) );

            //-- 
            this.HomeCell.IsHome = false;

            //-- Set the widget to the current cell again in order to place it into the cell's center
            this.CurrentCell.SetWidget( _widget );
        }

        public override string ToString()
        {
            //-- Assemble the main movement data
            string result = String.Format( "( Widget Name: {0}, Home Cell: {1}, Current Cell: {2} )", _widget.Name, this.HomeCell, this.CurrentCell );

            #region //-- Assemble the sub movement data

            if( ( _subMovements != null ) && ( _subMovements.Count > 0 ) )
            {
                result += String.Format( "\r\n\t=== Sub Movements ({0}) ===", _subMovements.Count );

                foreach( var subMovement in _subMovements )
                {
                    result += String.Format( "\r\n\t- {0}", subMovement );
                }
            }

            #endregion //-- Assemble the sub movement data

            return result;
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------