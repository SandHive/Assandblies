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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public abstract class SandWidgetGridCellBase : Border
    {
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets a flag that indicates whether the cell currently contains a 
        /// widget or not. 
        /// </summary>
        public abstract bool ContainsWidget { get; }
        
        /// <summary>
        /// Gets a flag that indicates whether this cell is currently the home 
        /// of a widget or not.
        /// </summary>
        public abstract bool IsHome { get; internal set; }

        /// <summary>
        /// Gets a flag that indicates whether this cell is currently hoverd by 
        /// a widget or not.
        /// </summary>
        public abstract bool IsHovered { get; internal set; }

        /// <summary>
        /// Gets the widget that is currently in the cell. 
        /// </summary>
        public abstract SandWidget Widget { get; internal set; }

        /// <summary>
        /// Gets the x cell index within the parent grid. 
        /// </summary>
        public int XCellIndex { get; protected set; }

        /// <summary>
        /// Gets the y cell index within the parent grid. 
        /// </summary>
        public int YCellIndex { get; protected set; }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Places the current widget to the cell's center. 
        /// </summary>
        internal abstract void CenterCurrentWidget();

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------