/* Copyright (c) 2013 The Sandmakers
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public class SandPanelItem : ContentControl
    {
        //---------------------------------------------------------------------
        #region Fields

        private readonly Guid  _guid = Guid.NewGuid();

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets a unique identifier for this object.
        /// </summary>
        public Guid Guid { get { return _guid; } }

        /// <summary>
        /// Gets the movement data where all information about the current item 
        /// movement is stored.
        /// </summary>
        internal SandPanelItemMovementData MovementData { get; private set; }

        #endregion Properties
        //---------------------------------------------------------------------
        #region ContentControl Members

        protected override void OnMouseDown( MouseButtonEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseDown( e );

            //-- Check for abortion
            if( e.Handled ) { return; }

            //-- Get the location of the current mouse movement
            Point mouseLocation = e.GetPosition( (SandPanel) this.Parent );

            //-- Calculate the offset between the upper left corner and the mouse location
            this.MovementData = new SandPanelItemMovementData();
            this.MovementData.MouseToItemLocationOffset = new Point(

                mouseLocation.X - SandPanel.GetLeft( this ),
                mouseLocation.Y - SandPanel.GetTop( this )
            );

            //-- Capture the mouse on the affected item (prevents that a too quick
            //-- mouse movement will loose the item)
            this.CaptureMouse();
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseMove( e );

            //-- Return when the offset point was not calculated before (may occur
            //-- when the mouse is pressed shortly beyond the TestItem and is moved
            //-- into it at once)
            if( ( this.MovementData == null ) || ( e.LeftButton != MouseButtonState.Pressed ) ) 
            {
                //-- Something is wrong, we shall abort here
                e.Handled = true;
                return; 
            }

            //-- Determine elementary TestItem positions
            var parentSandPanel = (SandPanel) this.Parent;
            Point mouseLocation = e.GetPosition( parentSandPanel );
            double itemLeft = mouseLocation.X - this.MovementData.MouseToItemLocationOffset.X;
            double itemRight = itemLeft + this.ActualWidth;
            double itemTop = mouseLocation.Y - this.MovementData.MouseToItemLocationOffset.Y;
            double itemBottom = itemTop + this.ActualHeight;
            #region double newX = ...

            double newX = itemLeft;

            if( itemLeft < 0 )
            {
                //-- The item would leave the left border of the Canvas object, so let's prevent that 
                newX = 0;
            }
            else if( itemRight > parentSandPanel.ActualWidth )
            {
                //-- The item would leave the right border of the Canvas object, so let's prevent that too
                newX = parentSandPanel.ActualWidth - this.ActualWidth;
            }

            #endregion double newX = ...
            #region double newY = ...

            double newY = itemTop;

            if( itemTop < 0 )
            {
                //-- The item would leave the top border of the Canvas object, so let's prevent that
                newY = 0;
            }
            else if( itemBottom > parentSandPanel.ActualHeight )
            {
                //-- The item would leave the bottom border of the Canvas object, so let's prevent that too
                newY = parentSandPanel.ActualHeight - this.ActualHeight;
            }

            #endregion double newY = ...

            //-- Apply the new location
            this.MovementData.Location = new Point( newX, newY );
            SandPanel.SetLeft( this, newX );
            SandPanel.SetTop( this, newY );
        }

        protected override void OnMouseUp( MouseButtonEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseUp( e );

            //-- Reset all made settings on TestItem
            this.MovementData = null;
            this.ReleaseMouseCapture();
        }

        #endregion ContentControl Members
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------