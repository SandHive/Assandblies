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
        #region Event Handling

        protected override void OnMouseDown( MouseButtonEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseDown( e );

            //-- Hide the mouse cursor ...
            Mouse.OverrideCursor = Cursors.None;
            //-- ... and show some nice effect around the affected item
            this.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                Direction = 320,
                ShadowDepth = 4,
                Opacity = 1
            };
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseMove( e );

            //-- Check for abortion
            if( e.Handled ) { return; }

            //-- Get the location of the current mouse movement
            Point mouseLocation = e.GetPosition( this.ParentSandPanel );

            if( ( mouseLocation.X < 0 ) || ( mouseLocation.X > base.ParentSandPanel.ActualWidth ) || ( mouseLocation.Y < 0 ) || ( mouseLocation.Y > base.ParentSandPanel.ActualHeight ) )
            {
                //-- Make the mouse cursor visible when it leaves the canvas
                Mouse.OverrideCursor = null;
            }
            else
            {
                Mouse.OverrideCursor = Cursors.None;
            }
        }

        protected override void OnMouseUp( MouseButtonEventArgs e )
        {
            //-- Call the base implementation
            base.OnMouseUp( e );

            //-- The mouse cursor was set to Cursors.None before, so let's reset it again
            Mouse.OverrideCursor = null;

            //-- Reset all made settings on TestItem
            this.Effect = null;
        }

        #endregion Event Handling
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------