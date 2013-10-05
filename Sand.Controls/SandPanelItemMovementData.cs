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
    /// <summary>
    /// Stores all relevant data about an item movement.
    /// </summary>
    internal sealed class SandPanelItemMovementData
    {
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets the home SandPanelWidgetGridCell object (that's the cell where
        /// the widget has started its movement).
        /// </summary>
        public SandPanelWidgetGridCell HomeWidgetGridCell { get; internal set; }

        /// <summary>
        /// Gets the current hovered SandPanelWidgetGridCell object.
        /// </summary>
        public SandPanelWidgetGridCell HoveredWidgetGridCell { get; internal set; }

        /// <summary>
        /// Gets the coordinates of the upper-left corner of the SandPanelItem 
        /// object.
        /// </summary>
        public Point Location { get; internal set; }

        /// <summary>
        /// Gets the offset between the item and mouse location.
        /// </summary>
        public Point MouseToItemLocationOffset { get; internal set; }

        #endregion Properties
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------