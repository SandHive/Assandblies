/* Copyright (c) 2013 The Sand Hive Project
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
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public sealed class SandPanelWidgetGridCell : Border, ISandPanelWidgetGridCell
    {
        //---------------------------------------------------------------------
        #region Events

        public static readonly RoutedEvent WidgetEnterEvent = EventManager.RegisterRoutedEvent( "WidgetEnter", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( SandPanelWidgetGridCell ) );
        /// <summary>
        /// Occurs when a widget enters the cell.
        /// </summary>
        public event RoutedEventHandler WidgetEnter
        {
            add 
            { 
                this.AddHandler( SandPanelWidgetGridCell.WidgetEnterEvent, value ); 
            }
            remove 
            { 
                this.RemoveHandler( SandPanelWidgetGridCell.WidgetEnterEvent, value ); 
            }
        }

        public static readonly RoutedEvent WidgetLeaveEvent = EventManager.RegisterRoutedEvent( "WidgetLeave", RoutingStrategy.Bubble, typeof( RoutedEventHandler ), typeof( SandPanelWidgetGridCell ) );
        /// <summary>
        /// Occurs when a widget leaves the cell.
        /// </summary>
        public event RoutedEventHandler WidgetLeave
        {
            add
            {
                this.AddHandler( SandPanelWidgetGridCell.WidgetLeaveEvent, value );
            }
            remove
            {
                this.RemoveHandler( SandPanelWidgetGridCell.WidgetLeaveEvent, value );
            }
        }

        #endregion Events
        //---------------------------------------------------------------------
        #region Fields

        private readonly Guid _guid = Guid.NewGuid();

        private SandPanelWidget _originalWidget;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets a unique identifier for this object.
        /// </summary>
        public Guid Guid { get { return _guid; } }

        public static DependencyProperty IsHoveredProperty = DependencyProperty.Register( "IsHovered", typeof( bool ), typeof( SandPanelWidgetGridCell ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets a flag that indicates whether this cell is currently hoverd by 
        /// a widget or not.
        /// </summary>
        public bool IsHovered
        {
            get { return (bool) this.GetValue( SandPanelWidgetGridCell.IsHoveredProperty ); }
            internal set { this.SetValue( SandPanelWidgetGridCell.IsHoveredProperty, value ); }
        }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandPanelWidgetGridCell class.
        /// </summary>
        /// <param name="xCellIndex">
        /// The x cell index within the widget grid.
        /// </param>
        /// <param name="yCellIndex">
        /// The y cell index within the widget grid.
        /// </param>
        internal SandPanelWidgetGridCell( int xCellIndex, int yCellIndex )
        {
            this.XCellIndex = xCellIndex;
            this.YCellIndex = yCellIndex;

            SandPanelWidgetGrid.SetZIndex( this, int.MinValue );
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region ISandPanelWidgetGridCell Members

        public bool ContainsWidget { get { return this.Widget != null; } }

        public static DependencyProperty IsHomeProperty = DependencyProperty.Register( "IsHome", typeof( bool ), typeof( SandPanelWidgetGridCell ), new PropertyMetadata( false ) );
        public bool IsHome
        {
            get { return (bool) this.GetValue( SandPanelWidgetGridCell.IsHomeProperty ); }
            set { this.SetValue( SandPanelWidgetGridCell.IsHomeProperty, value ); }
        }

        public void OnWidgetDropped( SandPanelWidget widget )
        {
            this.Widget = widget;
            this.IsHovered = false;

            #region //-- Place the widget to the cell's center

            //-- Get the location of the cell within the widget grid
            Point cellInGridLocation = ( (SandPanelWidgetGrid) this.Parent ).GetCellLocation( this );

            //-- Calculate the offset in order to center the widget
            double xOffset = ( this.Width - widget.Width ) / 2;
            double yOffset = ( this.Height - widget.Height ) / 2;

            //-- Move the widget to the cell's center
            SandPanelWidgetGrid.SetLeft( widget, cellInGridLocation.X + xOffset );
            SandPanelWidgetGrid.SetTop( widget, cellInGridLocation.Y + yOffset );

            #endregion //-- Place the widget to the cell's center

            //-- Update the home cell
            if( widget.HomeWidgetGridCell != null )
            {
                widget.HomeWidgetGridCell.IsHome = false;
                widget.HomeWidgetGridCell.Widget = null;
            }
            widget.HomeWidgetGridCell = this;
        }

        public void OnWidgetEnter( SandPanelWidget widget )
        {
            this.IsHovered = true;
            this.RaiseEvent( new RoutedEventArgs( SandPanelWidgetGridCell.WidgetEnterEvent ) );

            //-- Check if there is already a widget in this cell ...
            if( this.ContainsWidget )
            {
                //-- ... that is not identical with the current hovered one (results in bad behaviour otherwise) ...
                if( this.Widget != widget )
                {
                    //-- ... and we are not dragging back to our home grid cell ...
                    if( this != widget.HomeWidgetGridCell )
                    {
                        //-- ... then just switch both widgets
                        _originalWidget = this.Widget;
                        widget.HomeWidgetGridCell.OnWidgetDropped( this.Widget );
                        this.Widget = null;
                    }
                }
            }
        }

        public void OnWidgetLeave( SandPanelWidget widget )
        {
            this.IsHovered = false;
            this.RaiseEvent( new RoutedEventArgs( SandPanelWidgetGridCell.WidgetLeaveEvent ) );

            if( _originalWidget != null )
            {
                //-- Restore the original widget that was moved when the 
                //-- _hoverWidget had entered this cell
                this.OnWidgetDropped( _originalWidget );
                _originalWidget = null;
            }
        }

        public SandPanelWidget Widget { get; set; }

        public int XCellIndex { get; private set; }

        public int YCellIndex { get; private set; }

        #endregion ISandPanelWidgetGridCell Members
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------