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

using System.Windows;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public interface ISandPanelWidgetGridCell
    {
        /// <summary>
        /// Gets a flag that indicates whether the cell currently contains a 
        /// widget or not. 
        /// </summary>
        bool ContainsWidget { get; }

        /// <summary>
        /// Handles a dropped widget.
        /// </summary>
        /// <param name="widget">
        /// The dropped SandPanelWidget object.
        /// </param>
        void OnWidgetDropped( SandPanelWidget widget );

        /// <summary>
        /// Handles a widget that enters the cell..
        /// </summary>
        /// <param name="widget">
        /// The entering SandPanelWidget object.
        /// </param>
        void OnWidgetEnter( SandPanelWidget widget );

        /// <summary>
        /// Handles a widget that leaves the cell..
        /// </summary>
        /// <param name="widget">
        /// The leaving SandPanelWidget object.
        /// </param>
        void OnWidgetLeave( SandPanelWidget widget );

        /// <summary>
        /// Gets the position within the SandPanelWidgetGrid.
        /// </summary>
        SandPanelWidgetGridCellPosition PositionInGrid { get; }

        /// <summary>
        /// Gets or sets the widget that is currently in the cell. 
        /// </summary>
        SandPanelWidget Widget { get; set; }
    }
}
//-----------------------------------------------------------------------------