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
using System.Windows.Media;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    [Serializable]
    public struct SandWidgetAllocationInfo
    {
        //---------------------------------------------------------------------
        #region Fields

        /// <summary>
        /// Gets the tile size of the widget.
        /// </summary>
        public Size TileSize { get; private set; }

        /// <summary>
        /// Gets the top left point of the widget. 
        /// </summary>
        public Point TopLeft { get; private set; }

        /// <summary>
        /// Gets the widget name.
        /// </summary>
        public string WidgetName { get; private set; }

        #endregion Fields
        //---------------------------------------------------------------------
        #region Constructors

        public SandWidgetAllocationInfo( string widgetName, Point topLeft, Size tileSize )
            : this()
        {
            //-- Apply arguments
            this.WidgetName = widgetName;
            this.TopLeft = topLeft;
            this.TileSize = tileSize;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------
