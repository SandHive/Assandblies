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
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    /// <summary>
    /// Manages the positioning of widgets inside one or several widget grids. 
    /// </summary>
    public static class SandPanelWidgetPositioner
    {
        //---------------------------------------------------------------------
        #region Fields

        private static Dictionary<Guid, SandPanelWidgetGrid> _widgetGrids = new Dictionary<Guid, SandPanelWidgetGrid>();

        #endregion Fields
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Deregisters a SandPanelWidgetGrid again.
        /// </summary>
        /// <param name="grid">
        /// The SandPanelWidgetGrid object.
        /// </param>
        public static void DeregisterGrid( SandPanelWidgetGrid grid )
        {
            if( _widgetGrids.ContainsKey( grid.Guid ) )
            {
                _widgetGrids.Remove( grid.Guid );
            }
        }

        /// <summary>
        /// Registers a SandPanelWidgetGrid to this manager.
        /// </summary>
        /// <param name="grid">
        /// The SandPanelWidgetGrid object.
        /// </param>
        public static void RegisterGrid( SandPanelWidgetGrid grid )
        {
            _widgetGrids.Add( grid.Guid, grid );
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------
