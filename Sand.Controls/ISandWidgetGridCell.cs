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

using System.Windows;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public interface ISandWidgetGridCell
    {
        /// <summary>
        /// Gets a flag that indicates whether the cell currently contains a 
        /// widget or not. 
        /// </summary>
        bool ContainsWidget { get; }
       
        /// <summary>
        /// Gets or sets a flag that indicates whether this cell is currently 
        /// the home of a widget or not.
        /// </summary>
        bool IsHome { get; set; }

        /// <summary>
        /// Handles a dropped widget.
        /// </summary>
        /// <param name="widget">
        /// The SandWidget object that should be dropped into this cell.
        /// </param>
        void OnWidgetDropped( SandWidget widget );

        /// <summary>
        /// Handles the entering of a widget.
        /// </summary>
        /// <param name="widget">
        /// The SandWidget object that enters this cell.
        /// </param>
        void OnWidgetEnter( SandWidget widget );

        /// <summary>
        /// Handles the entering of a widget.
        /// </summary>
        /// <param name="widget">
        /// The SandWidget object that enters this cell.
        /// </param>
        /// <param name="isPrimaryMovingWidget">
        /// A flag that indicates whether the widget is the primary moving 
        /// widget (the one that is moved by a user interaction) or a
        /// secondary moving widget (one of the widgets that were displaced
        /// by the primary one).
        /// </param>
        void OnWidgetEnter( SandWidget widget, bool isPrimaryMovingWidget );

        /// <summary>
        /// Handles the leaving of a widget.
        /// </summary>
        void OnWidgetLeave();

        /// <summary>
        /// Gets or sets the widget that is currently in the cell. 
        /// </summary>
        SandWidget Widget { get; }

        /// <summary>
        /// Gets the x cell index within the parent grid. 
        /// </summary>
        int XCellIndex { get; }

        /// <summary>
        /// Gets the x cell index within the parent grid. 
        /// </summary>
        int YCellIndex { get; }
    }
}
//-----------------------------------------------------------------------------