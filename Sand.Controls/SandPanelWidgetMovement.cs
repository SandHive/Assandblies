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
        /// Gets or sets the current hovered ISandPanelWidgetGridCell object.
        /// </summary>
        public ISandPanelWidgetGridCell HoveredWidgetGridCell { get; set; }

        /// <summary>
        /// Gets the widget to which this movement belongs.
        /// </summary>
        public SandPanelWidget MovingWidget { get; private set; }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandPanelWidgetMovement class.
        /// </summary>
        /// <param name="widget">
        /// The widget that is moving.
        /// </param>
        public SandPanelWidgetMovement( SandPanelWidget widget )
        {
            this.MovingWidget = widget;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------