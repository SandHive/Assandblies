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

using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public class SandWidget : SandPanelItem
    {
        //---------------------------------------------------------------------
        #region Fields

        private SandWidgetMovement _movement;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        public static DependencyProperty MouseDownEffectProperty = DependencyProperty.Register( "MouseDownEffect", typeof( Effect ), typeof( SandWidget ), new PropertyMetadata( new DropShadowEffect { Color = Colors.Black, Direction = 320, ShadowDepth = 4, Opacity = 1 } ) );
        /// <summary>
        /// Gets or sets the effect when a mouse button is pressed.
        /// </summary>
        public Effect MouseDownEffect
        {
            get { return (Effect) this.GetValue( SandWidget.MouseDownEffectProperty ); }
            set { this.SetValue( SandWidget.MouseDownEffectProperty, value ); }
        }

        public static DependencyProperty TileSizeProperty = DependencyProperty.Register( "TileSize", typeof( Size ), typeof( SandWidget ), new PropertyMetadata( new Size( 1.0, 1.0 ) ) );
        /// <summary>
        /// Gets or sets the tile size.
        /// </summary>
        public Size TileSize
        {
            get { return (Size) this.GetValue( SandWidget.TileSizeProperty ); }
            set { this.SetValue( SandWidget.TileSizeProperty, value ); }
        }

        #endregion Properties
        //---------------------------------------------------------------------
        #region SandPanelItem Members

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

            //-- Initialize a new widget movement
            var homeGridCell = ( (SandWidgetGrid) this.Parent ).GetOccupiedGridCell( this );
            homeGridCell.IsHome = true;
            homeGridCell.OnWidgetEnter( this );

            _movement = new SandWidgetMovement( this, homeGridCell );

            Debug.WriteLine( string.Format( "Widget moving started (Name: {0}; Cell: {1})", this.Name, homeGridCell ) );
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

            //-- The mouse is captured in the SandPanelItem.OnMouseDown method
            //-- which will result in a firing of the MouseMove event although
            //-- the MouseDown event handler is not completely processed
            if( _movement == null ) { return; }

            //-- Validate the movement by rearranging the positions of all affected widgets 
            var hoveredCell = ( (SandWidgetGrid) this.Parent ).GetOccupiedGridCell( this );
            SandWidgetPositioner.ValidateWidgetMovement( _movement, hoveredCell );
        }

        protected override void OnMouseUp( MouseButtonEventArgs e )
        {
            //-- The movement may be null when the mouse is pressed on an empty cell
            //-- and will be released on another cell's widget
            if( _movement == null ) { return; }

            Debug.WriteLine( string.Format( "Widget moving stopped (Name: {0}; Cell: {1})", this.Name, _movement.CurrentCell ) );
            
            //-- Finish the movement (should be done first, before all data is reset)
            _movement.HomeCell.IsHome = false;
            _movement.CurrentCell.OnWidgetDropped( this );
            
            //-- Call the base implementation
            base.OnMouseUp( e );

            //-- The mouse cursor was set to Cursors.None before, so let's reset it again
            Mouse.OverrideCursor = null;

            //-- Reset all made settings
            this.Effect = null;
            _movement = null;
        }

        #endregion SandPanelItem Members
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------