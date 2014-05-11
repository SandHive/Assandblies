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
using System.Windows.Controls;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public sealed class SandWidgetGridCell : Border, ISandWidgetGridCell
    {
        //---------------------------------------------------------------------
        #region Events

        public static readonly RoutedEvent WidgetEnterEvent = EventManager.RegisterRoutedEvent( "WidgetEnter", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( SandWidgetGridCell ) );
        /// <summary>
        /// Occurs when a widget enters the cell.
        /// </summary>
        public event RoutedEventHandler WidgetEnter
        {
            add 
            { 
                this.AddHandler( SandWidgetGridCell.WidgetEnterEvent, value ); 
            }
            remove 
            { 
                this.RemoveHandler( SandWidgetGridCell.WidgetEnterEvent, value ); 
            }
        }

        public static readonly RoutedEvent WidgetLeaveEvent = EventManager.RegisterRoutedEvent( "WidgetLeave", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( SandWidgetGridCell ) );
        /// <summary>
        /// Occurs when a widget leaves the cell.
        /// </summary>
        public event RoutedEventHandler WidgetLeave
        {
            add
            {
                this.AddHandler( SandWidgetGridCell.WidgetLeaveEvent, value );
            }
            remove
            {
                this.RemoveHandler( SandWidgetGridCell.WidgetLeaveEvent, value );
            }
        }

        #endregion Events
        //---------------------------------------------------------------------
        #region Fields

        private readonly Guid _guid = Guid.NewGuid();

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets a unique identifier for this object.
        /// </summary>
        public Guid Guid { get { return _guid; } }

        public static DependencyProperty IsHoveredProperty = DependencyProperty.Register( "IsHovered", typeof( bool ), typeof( SandWidgetGridCell ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets a flag that indicates whether this cell is currently hoverd by 
        /// a widget or not.
        /// </summary>
        public bool IsHovered
        {
            get { return (bool) this.GetValue( SandWidgetGridCell.IsHoveredProperty ); }
            internal set { this.SetValue( SandWidgetGridCell.IsHoveredProperty, value ); }
        }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandWidgetGridCell class.
        /// </summary>
        /// <param name="xCellIndex">
        /// The x cell index within the widget grid.
        /// </param>
        /// <param name="yCellIndex">
        /// The y cell index within the widget grid.
        /// </param>
        internal SandWidgetGridCell( int xCellIndex, int yCellIndex )
        {
            this.XCellIndex = xCellIndex;
            this.YCellIndex = yCellIndex;

            SandWidgetGrid.SetZIndex( this, int.MinValue );
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region ISandWidgetGridCell Members

        public bool ContainsWidget { get { return this.Widget != null; } }

        public static DependencyProperty IsHomeProperty = DependencyProperty.Register( "IsHome", typeof( bool ), typeof( SandWidgetGridCell ), new PropertyMetadata( false ) );
        public bool IsHome
        {
            get { return (bool) this.GetValue( SandWidgetGridCell.IsHomeProperty ); }
            set { this.SetValue( SandWidgetGridCell.IsHomeProperty, value ); }
        }

        [DebuggerStepThrough]
        public void OnWidgetDropped( SandWidget droppedWidget )
        {
            this.OnWidgetDropped( droppedWidget, false );
        }

        public void OnWidgetEnter( SandWidget enteringWidget )
        {
            this.IsHovered = true;
            this.RaiseEvent( new RoutedEventArgs( SandWidgetGridCell.WidgetEnterEvent ) );
        }

        public void OnWidgetLeave( SandWidget leavingWidget )
        {
            if( this.Widget != leavingWidget )
            {
                throw new ArgumentException( "Invalid leaving widget!" );
            }

            this.IsHovered = false;
            this.Widget = null;
            this.RaiseEvent( new RoutedEventArgs( SandWidgetGridCell.WidgetLeaveEvent ) );
        }

        public SandWidget Widget { get; set; }

        public int XCellIndex { get; private set; }

        public int YCellIndex { get; private set; }

        #endregion ISandWidgetGridCell Members
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Handles a dropped widget.
        /// </summary>
        /// <param name="droppedWidget">
        /// The dropped SandWidget object.
        /// </param>
        /// <param name="keepIsHoveredEnabled">
        /// A flag that indicates whether the IsHovered property should be kept
        /// enabled or not.
        /// </param>
        internal void OnWidgetDropped( SandWidget droppedWidget, bool keepIsHoveredEnabled )
        {
            if( //-- Check that this cell is not occupied ...
                ( this.Widget != null ) &&
                //-- ... but consider the case when moving back to the own home cell
                ( this.Widget != droppedWidget ) )
                throw new InvalidOperationException( "The cell is already occupied!" );

            this.Widget = droppedWidget;
            this.IsHovered = keepIsHoveredEnabled;

            #region //-- Place the widget to the cell's center

            //-- Get the location of the cell within the widget grid
            Point cellInGridLocation = ( (SandWidgetGrid) this.Parent ).GetCellLocation( this );

            //-- Calculate the offset in order to center the widget
            double xOffset = ( this.Width - droppedWidget.Width ) / 2;
            double yOffset = ( this.Height - droppedWidget.Height ) / 2;

            //-- Move the widget to the cell's center
            SandWidgetGrid.SetLeft( droppedWidget, cellInGridLocation.X + xOffset );
            SandWidgetGrid.SetTop( droppedWidget, cellInGridLocation.Y + yOffset );

            #endregion //-- Place the widget to the cell's center
        }

        /// <summary>
        /// Create some more informative output.
        /// </summary>
        /// <returns>
        /// The string representation of this object.
        /// </returns>
        public override string ToString()
        {
            return String.Format( "({0},{1})", this.XCellIndex, this.YCellIndex );
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------