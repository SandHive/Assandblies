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
    public class SandPanelWidget : SandPanelItem
    {
        //---------------------------------------------------------------------
        #region Properties

        internal bool IsMoving { get; private set; }

        public static DependencyProperty MouseDownEffectProperty = DependencyProperty.Register( "MouseDownEffect", typeof( Effect ), typeof( SandPanelWidget ), new PropertyMetadata( new DropShadowEffect { Color = Colors.Black, Direction = 320, ShadowDepth = 4, Opacity = 1 } ) );
        /// <summary>
        /// Gets or sets the effect when a mouse button is pressed.
        /// </summary>
        public Effect MouseDownEffect
        {
            get { return (Effect) this.GetValue( SandPanelWidget.MouseDownEffectProperty ); }
            set { this.SetValue( SandPanelWidget.MouseDownEffectProperty, value ); }
        }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Event Handling

        protected override void OnMouseDown( MouseButtonEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseDown( e );

            //-- Check for abortion
            if( e.Handled ) { return; }

            //-- Hide the mouse cursor ...
            Mouse.OverrideCursor = Cursors.None;
            //-- ... and show some nice effect around the affected item
            this.Effect = this.MouseDownEffect;

            //-- Inform the widget grid about the started moving
            ( (SandPanelWidgetGrid) this.Parent ).OnWidgetMovingStarted( this );

            this.IsMoving = true;
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseMove( e );

            //-- Check for abortion
            if( e.Handled ) { return; }

            //-- Get the location of the current mouse movement
            var parentSandPanel = (SandPanel) this.Parent;
            Point mouseLocation = e.GetPosition( parentSandPanel );

            if( ( mouseLocation.X < 0 ) || ( mouseLocation.X > parentSandPanel.ActualWidth ) || ( mouseLocation.Y < 0 ) || ( mouseLocation.Y > parentSandPanel.ActualHeight ) )
            {
                //-- Make the mouse cursor visible when it leaves the canvas
                Mouse.OverrideCursor = null;
            }
            else
            {
                Mouse.OverrideCursor = Cursors.None;
            }

            //-- Inform the widget grid about the movement
            ( (SandPanelWidgetGrid) this.Parent ).OnWidgetMove( this );
        }

        protected override void OnMouseUp( MouseButtonEventArgs e )
        {
            //-- Inform the widget grid about the stopped moving (should be done 
            //-- first, before all data is reset)
            ( (SandPanelWidgetGrid) this.Parent ).OnWidgetMovingStopped( this );

            //-- Call the base implementation
            base.OnMouseUp( e );

            //-- The mouse cursor was set to Cursors.None before, so let's reset it again
            Mouse.OverrideCursor = null;

            //-- Reset all made settings
            this.Effect = null;
            this.IsMoving = false;
        }

        #endregion Event Handling
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------