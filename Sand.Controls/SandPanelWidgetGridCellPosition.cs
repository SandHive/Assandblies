﻿/* Copyright (c) 2013 The Sand Hive Project
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
    /// A simple struct for saving the cell position in the grid. 
    /// </summary>
    public struct SandPanelWidgetGridCellPosition
    {
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets or sets the top left x position of the cell.
        /// </summary>
        public int TopLeftX { get; set; }

        /// <summary>
        /// Gets or sets the top left y position of the cell.
        /// </summary>
        public int TopLeftY { get; set; }

        /// <summary>
        /// Gets or sets the bottom right x position of the cell.
        /// </summary>
        public int BottomRightX { get; set; }

        /// <summary>
        /// Gets or sets the bottom right y position of the cell.
        /// </summary>
        public int BottomRightY { get; set; }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Object Members

        public override string ToString()
        {
            return string.Concat( "LeftTop {", this.TopLeftX, ",", this.TopLeftY, "}, RightBottom {", this.BottomRightX, ",", this.BottomRightY, "}" );
        }

        #endregion Object Members
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------