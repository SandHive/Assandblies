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
using System.Windows.Media;
using System.Windows.Media.Effects;
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
        /// Gets the parent SandPanel object.
        /// </summary>
        protected SandPanel ParentSandPanel { get; private set; }

        #endregion Properties
        //---------------------------------------------------------------------
        #region ContentControl Members

        public override void OnApplyTemplate()
        {
            //-- Call the base implementation
            base.OnApplyTemplate();

            //-- Apply the parent SandPanel object when it was not applied before
            this.ParentSandPanel = (SandPanel) this.Parent;
        }

        protected override void OnMouseDown( MouseButtonEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseDown( e );

            //-- Check for abortion
            if( e.Handled ) { return; }

            //-- Get the location of the current mouse movement
            Point mouseLocation = e.GetPosition( this.ParentSandPanel );

            //-- Calculate the offset between the upper left corner and the mouse location
            this.Tag = new Point(

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
            if( ( this.Tag == null ) || ( e.LeftButton != MouseButtonState.Pressed ) ) 
            {
                //-- Something is wrong, we shall abort here
                e.Handled = true;
                return; 
            }

            //-- Determine elementary TestItem positions
            Point mouseLocation = e.GetPosition( this.ParentSandPanel );
            Point offset = (Point) this.Tag;
            double itemLeft = mouseLocation.X - offset.X;
            double itemRight = itemLeft + this.ActualWidth;
            double itemTop = mouseLocation.Y - offset.Y;
            double itemBottom = itemTop + this.ActualHeight;

            #region //-- Handle the x-position of the TestItem

            if( itemLeft < 0 )
            {
                //-- The item would leave the left border of the Canvas object, so let's prevent that 
                SandPanel.SetLeft( this, 0 );
            }
            else if( itemRight > this.ParentSandPanel.ActualWidth )
            {
                //-- The item would leave the right border of the Canvas object, so let's prevent that too
                SandPanel.SetLeft( this, this.ParentSandPanel.ActualWidth - this.ActualWidth );
            }
            else
            {
                //-- The item is within the x-axe borders, so let's just apply its new x-position
                SandPanel.SetLeft( this, itemLeft );
            }

            #endregion //-- Handle the x-position of the TestItem

            #region //-- Handle the y-position of the TestItem

            if( itemTop < 0 )
            {
                //-- The item would leave the top border of the Canvas object, so let's prevent that
                SandPanel.SetTop( this, 0 );
            }
            else if( itemBottom > this.ParentSandPanel.ActualHeight )
            {
                //-- The item would leave the bottom border of the Canvas object, so let's prevent that too
                SandPanel.SetTop( this, this.ParentSandPanel.ActualHeight - this.ActualHeight );
            }
            else
            {
                //-- The item is within the y-axe borders, so let's just apply its new y-position
                SandPanel.SetTop( this, itemTop );
            }

            #endregion //-- Handle the y-position of the TestItem
        }

        protected override void OnMouseUp( MouseButtonEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseUp( e );

            //-- Reset all made settings on TestItem
            this.ReleaseMouseCapture();
            this.Tag = null;
        }

        #endregion ContentControl Members
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------