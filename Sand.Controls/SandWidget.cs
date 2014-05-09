﻿/* Copyright (c) 2013 - 2014 The Sandhive Project (http://sandhive.org)
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

        private bool _isManualModeEnabled;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        public static DependencyProperty IsMovingProperty = DependencyProperty.Register( "IsMoving", typeof( bool ), typeof( SandWidget ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets a flag that indicates whether the widget is moving or not.
        /// </summary>
        public bool IsMoving
        {
            get { return (bool) this.GetValue( SandWidget.IsMovingProperty ); }
            private set { this.SetValue( SandWidget.IsMovingProperty, value ); }
        }

        /// <summary>
        /// Gets the widget movement object.
        /// </summary>
        internal SandWidgetMovement Movement { get; private set; }

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
            //-- Do not evaluate the mouse movement when the widget is moved manually
            if( _isManualModeEnabled ) { return; }

            //-- Call the base implementation
            base.OnMouseDown( e );

            //-- Check for abortion
            if( e.Handled ) { return; }

            //-- Hide the mouse cursor
            Mouse.OverrideCursor = Cursors.None;

            //-- Start a new movement
            this.StartMovement();
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            //-- Do not evaluate the mouse movement when the widget is moved manually
            if( _isManualModeEnabled ) { return; }

            //-- Call the base implementation
            base.OnMouseMove( e );

            //-- Check for abortion
            if( e.Handled ) { return; }

            //-- Get the location of the current mouse movement
            var grid = (SandWidgetGrid) this.Parent;
            Point mouseLocation = e.GetPosition( grid );

            if( ( mouseLocation.X < 0 ) || ( mouseLocation.X > grid.ActualWidth ) || ( mouseLocation.Y < 0 ) || ( mouseLocation.Y > grid.ActualHeight ) )
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
            if( this.Movement == null ) { return; }

            //-- Validate the movement by rearranging the positions of all affected widgets 
            var hoveredCell = grid.GetOccupiedGridCell( this );
            this.Movement.MoveWidgetTo( hoveredCell );
        }

        protected override void OnMouseUp( MouseButtonEventArgs e )
        {
            //-- Do not evaluate the mouse movement when the widget is moved manually
            if( _isManualModeEnabled ) { return; }

            //-- The movement may be null when the mouse is pressed on an empty cell
            //-- and will be released on another cell's widget
            if( this.Movement == null ) { return; }

            this.Movement.Stop();
            
            //-- Call the base implementation
            base.OnMouseUp( e );

            //-- The mouse cursor was set to Cursors.None before, so let's reset it again
            Mouse.OverrideCursor = null;

            //-- Reset all made settings
            this.IsMoving = false;
            this.Movement = null;
        }

        #endregion SandPanelItem Members
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Starts a new movement.
        /// </summary>
        [DebuggerStepThrough]
        internal void StartMovement()
        {
            this.StartMovement( false );
        }

        /// <summary>
        /// Starts a new movement.
        /// </summary>
        /// <param name="isManualModeEnabled">
        /// A flag that indicates whether the widget is moved manually or not.
        /// </param>
        internal void StartMovement( bool isManualModeEnabled )
        {
            //-- Apply argument
            _isManualModeEnabled = isManualModeEnabled;

            //-- Initialize a new widget movement
            var homeGridCell = ( (SandWidgetGrid) this.Parent ).GetOccupiedGridCell( this );
            this.Movement = SandWidgetMovement.Start( this, homeGridCell );
            this.IsMoving = true;

            Debug.WriteLine( string.Format( "Widget moving started (Name: {0}; Cell: {1})", this.Name, homeGridCell ) );
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------