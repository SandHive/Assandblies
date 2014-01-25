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
    internal sealed class SandPanelWidgetMovement
    {
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets or sets the current ISandPanelWidgetGridCell object.
        /// </summary>
        public ISandPanelWidgetGridCell CurrentCell { get; set; }

        /// <summary>
        /// Gets the home ISandPanelWidgetGridCell object (that's the cell where
        /// the widget has started its movement).
        /// </summary>
        public ISandPanelWidgetGridCell HomeCell { get; private set; }

        public SortedList<Guid, SandPanelWidgetMovement> SubMovements { get; private set; }

        /// <summary>
        /// Gets the widget to which this movement belongs.
        /// </summary>
        public SandPanelWidget Widget { get; private set; }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandPanelWidgetMovement class.
        /// </summary>
        /// <param name="widget">
        /// The widget that is moving.
        /// </param>
        /// <param name="homeCell">
        /// The home cell of the moving widget.
        /// </param>
        public SandPanelWidgetMovement( SandPanelWidget widget, ISandPanelWidgetGridCell homeCell )
        {
            this.Widget = widget;
            this.HomeCell = homeCell;
            this.CurrentCell = homeCell;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Methods

        internal void AddSubMovement( SandPanelWidgetMovement subMovement )
        {
            if( this.SubMovements == null )
            {
                this.SubMovements = new SortedList<Guid, SandPanelWidgetMovement>();
            }

            try
            {
                //-- Do not test if the sub movement is in the dictionary, because 
                //-- we would throw an exeption anyway
                this.SubMovements.Add( subMovement.Widget.Guid, subMovement );
            }
            catch
            {
                Debug.WriteLine( String.Format( "Exception when adding sub movement {0} to movement {1}", subMovement, this ), "EXCEPTION" );
            }
        }

        internal void RemoveSubMovement( SandPanelWidgetMovement subMovement )
        {
            if( this.SubMovements == null )
            {
                //-- Do not test if the sub movement is in the dictionary, because 
                //-- we would throw an exeption anyway
                this.SubMovements.Remove( subMovement.Widget.Guid );
            }
        }

        public override string ToString()
        {
            //-- Assemble the main movement data
            string result = String.Format( "( Widget Name: {0}, Home Cell: {1}, Current Cell: {2} )", this.Widget.Name, this.HomeCell, this.CurrentCell );

            #region //-- Assemble the sub movement data

            if( ( this.SubMovements != null ) && ( this.SubMovements.Count > 0 ) )
            {
                result += String.Format( "\r\n\t=== Sub Movements ({0}) ===", this.SubMovements.Count );

                foreach( var subMovement in this.SubMovements )
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