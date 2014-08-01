/* Copyright (c) 2013 - 2014 The Sandhive Project (http://sandhive.org)
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
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    internal sealed class SandWidgetWrapper : SandPanelItem
    {
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets the home cell.
        /// </summary>
        public SandWidgetGridCellBase HomeCell { get; internal set; }

        public static DependencyProperty IsMovingProperty = DependencyProperty.Register( "IsMoving", typeof( bool ), typeof( SandWidgetWrapper ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets a flag that indicates whether the widget is moving or not.
        /// </summary>
        public bool IsMoving
        {
            get { return (bool) this.GetValue( SandWidgetWrapper.IsMovingProperty ); }
            private set { this.SetValue( SandWidgetWrapper.IsMovingProperty, value ); }
        }

        /// <summary>
        /// Gets the widget movement object.
        /// </summary>
        internal SandWidgetMovement Movement { get; private set; }

        /// <summary>
        /// Gets or sets the moving mode of the current widget.
        /// </summary>
        internal static SandWidgetMovementModes MovementMode { get; set; }

        /// <summary>
        /// Gets the parent SandWidgetGrid object.
        /// </summary>
        public SandWidgetGrid ParentGrid { get; private set; }

        /// <summary>
        /// Gets the ISandWidget object that is hosted by this instance.
        /// </summary>
        internal ISandWidget Widget { get; private set; }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandWidget class.
        /// </summary>
        /// <param name="widget">
        /// The ISandWidget object that is hosted by this instance.
        /// </param>
        internal SandWidgetWrapper( ISandWidget widget )
        {
            //-- Apply the ISandWidget object ... 
            this.Widget = widget;

            //-- .. and take its framework element as content
            this.Content = widget.WidgetFrameworkElement;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region SandPanelItem Members

        protected override void OnInitialized( EventArgs e )
        {
            base.OnInitialized( e );

            //-- Create a shortcut to the parent grid
            this.ParentGrid = (SandWidgetGrid) this.Parent;
        }

        protected override void OnMouseDown( MouseButtonEventArgs e )
        {
            //-- Do not evaluate the mouse movement when the widget is moved manually
            if( SandWidgetWrapper.MovementMode != SandWidgetMovementModes.DragAndDrop ) { return; }

            //-- Call the base implementation
            base.OnMouseDown( e );

            //-- Check for abortion
            if( e.Handled ) { return; }

            //-- Hide the mouse cursor
            Mouse.OverrideCursor = Cursors.None;

            //-- Start a new movement
            this.StartMovement( true );
        }

        protected override void OnMouseMove( MouseEventArgs e )
        {
            //-- Do not evaluate the mouse movement when the widget is moved manually
            if( SandWidgetWrapper.MovementMode != SandWidgetMovementModes.DragAndDrop ) { return; }

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
            if( SandWidgetWrapper.MovementMode != SandWidgetMovementModes.DragAndDrop ) { return; }

            //-- The movement may be null when the mouse is pressed on an empty cell
            //-- and will be released on another cell's widget
            if( this.Movement == null ) { return; }

            this.StopMovement();
            
            //-- Call the base implementation
            base.OnMouseUp( e );

            //-- The mouse cursor was set to Cursors.None before, so let's reset it again
            Mouse.OverrideCursor = null;
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
        /// <param name="isHomeGridCellHovered">
        /// A flag that indicates whether the "IsHovered" property of the home 
        /// grid cell should be set or not. 
        /// </param>
        private void StartMovement( bool isHomeGridCellHovered )
        {
            //-- Initialize a new widget movement
            var homeGridCell = ( (SandWidgetGrid) this.Parent ).GetOccupiedGridCell( this );
            homeGridCell.IsHovered = isHomeGridCellHovered;
            this.Movement = SandWidgetMovement.Start( this, homeGridCell );
            this.IsMoving = true;

            Debug.WriteLine( string.Format( "Widget moving started (Name: {0}; Cell: {1})", this.Name, homeGridCell ) );
        }

        /// <summary>
        /// Stops the widget moving.
        /// </summary>
        internal void StopMovement()
        {
            //-- Stop and reset the movement
            this.Movement.Stop();
            this.Movement.CurrentCell.IsHovered = false;
            this.Movement = null;

            //-- Reset other members
            this.IsMoving = false;
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------