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
    /// Manages the positioning of widgets inside one or several widget grids. 
    /// </summary>
    internal static class SandWidgetPositioner
    {
        //---------------------------------------------------------------------
        #region Fields

        private static Dictionary<Guid, SandWidgetGrid> _widgetGrids = new Dictionary<Guid, SandWidgetGrid>();

        #endregion Fields
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Deregisters a SandWidgetGrid again.
        /// </summary>
        /// <param name="grid">
        /// The SandWidgetGrid object.
        /// </param>
        public static void DeregisterGrid( SandWidgetGrid grid )
        {
            if( _widgetGrids.ContainsKey( grid.Guid ) )
            {
                _widgetGrids.Remove( grid.Guid );
            }
        }

        /// <summary>
        /// Registers a SandWidgetGrid to this manager.
        /// </summary>
        /// <param name="grid">
        /// The SandWidgetGrid object.
        /// </param>
        public static void RegisterGrid( SandWidgetGrid grid )
        {
            _widgetGrids.Add( grid.Guid, grid );
        }

        /// <summary>
        /// Validates a widget movement by checking and correcting all widget 
        /// positions.
        /// </summary>
        /// <param name="widgetMovement">
        /// The data of the moving widget and all other widgets that have been
        /// moved.
        /// </param>
        /// <param name="newHoveredCell">
        /// The new hovered cell.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The widget movement or new hovered cell may not be null.
        /// </exception>
        public static void ValidateWidgetMovement( SandWidgetMovement widgetMovement, ISandWidgetGridCell newHoveredCell )
        {
            if( ( widgetMovement == null ) || ( newHoveredCell == null ) )
            {
                throw new ArgumentNullException( "The widget movement or new hovered cell may not be null!" );
            }

            //-- Do nothing when the cell did not changed
            if( ISandWidgetGridCell.Equals( widgetMovement.CurrentCell, newHoveredCell ) ) { return; }

            Debug.WriteLine( String.Format( "Validating Widget Movement \r\n\tSTART DATA\r\n\t{0}", widgetMovement ) );

            if( newHoveredCell.XCellIndex == 0 && newHoveredCell.YCellIndex == 0 )
            {

            }

            //-- Handle the hovered cell change 
            widgetMovement.CurrentCell.OnWidgetLeave( widgetMovement.Widget );
            widgetMovement.CurrentCell.Widget = null;
            widgetMovement.CurrentCell = newHoveredCell;
            newHoveredCell.OnWidgetEnter( widgetMovement.Widget );
            
            //-- Check if there is already a widget in this cell ...
            SandWidgetMovement subWidgetMovement = null;
            if( newHoveredCell.ContainsWidget )
            {
                if( widgetMovement.SubMovements != null )
                {
                    if( !widgetMovement.SubMovements.ContainsKey( newHoveredCell.Widget.Guid ) )
                    {
                        
                    }
                }

                subWidgetMovement = new SandWidgetMovement( newHoveredCell.Widget, newHoveredCell );
                subWidgetMovement.CurrentCell = widgetMovement.HomeCell;
                subWidgetMovement.CurrentCell.OnWidgetDropped( newHoveredCell.Widget );

                newHoveredCell.Widget = widgetMovement.Widget;
            }

            if( widgetMovement.SubMovements != null )
            {
                var subMovements = widgetMovement.SubMovements.Values;
                for( int i = 0; i < subMovements.Count; i++ )
                {
                    if( subWidgetMovement == subMovements[i] ) { continue; }

                    if( !subMovements[i].HomeCell.ContainsWidget )
                    {
                        if( subWidgetMovement != null && subWidgetMovement.Widget == subMovements[i].Widget )
                        {
                            subWidgetMovement = null;
                        }

                        //-- Move the moved widget back to its home cell when it is free again 
                        subMovements[i].CurrentCell.Widget = null;
                        subMovements[i].HomeCell.OnWidgetDropped( subMovements[i].Widget );
                        widgetMovement.SubMovements.Remove( subMovements[i].Widget.Guid );
                        i--;
                    }
                }
            }

            if( subWidgetMovement != null )
            {
                widgetMovement.AddSubMovement( subWidgetMovement );
            }

            Debug.WriteLine( String.Format( "\r\n\tRESULT\r\n\t{0}", widgetMovement ) );
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------
