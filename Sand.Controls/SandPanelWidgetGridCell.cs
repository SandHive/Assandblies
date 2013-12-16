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

        public static DependencyProperty IsWidgetOverProperty = DependencyProperty.Register( "IsWidgetOver", typeof( bool ), typeof( SandPanelWidgetGridCell ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets or sets the height of a cell.
        /// </summary>
        public bool IsWidgetOver
        {
            get { return (bool) this.GetValue( SandPanelWidgetGridCell.IsWidgetOverProperty ); }
            internal set { this.SetValue( SandPanelWidgetGridCell.IsWidgetOverProperty, value ); }
        }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandPanelWidgetGridCell class.
        /// </summary>
        /// <param name="xPosInGrid">
        /// The x position within the widget grid.
        /// </param>
        /// <param name="yPosInGrid">
        /// The y position within the widget grid.
        /// </param>
        internal SandPanelWidgetGridCell( int xPosInGrid, int yPosInGrid )
        {
            this.PositionInGrid = new SandPanelWidgetGridCellPosition()
            {
                TopLeftX = xPosInGrid,
                TopLeftY = yPosInGrid
            };

            SandPanelWidgetGrid.SetZIndex( this, int.MinValue );
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region ISandPanelWidgetGridCell Members

        public bool ContainsWidget { get { return this.Widget != null; } }

        public void OnWidgetDropped( SandPanelWidget widget )
        {
            this.Widget = widget;
            this.IsWidgetOver = false;

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

            if( widget.HomeWidgetGridCell != null )
            {
                //-- Reset the "Widget" property of the old home cell
                widget.HomeWidgetGridCell.Widget = null;
            }

            //-- Update the home cell
            widget.HomeWidgetGridCell = this;
        }

        public void OnWidgetEnter( SandPanelWidget widget )
        {
            this.IsWidgetOver = true;
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
            this.IsWidgetOver = false;
            this.RaiseEvent( new RoutedEventArgs( SandPanelWidgetGridCell.WidgetLeaveEvent ) );

            if( _originalWidget != null )
            {
                //-- Restore the original widget that was moved when the 
                //-- _hoverWidget had entered this cell
                this.OnWidgetDropped( _originalWidget );
                _originalWidget = null;
            }
        }

        public SandPanelWidgetGridCellPosition PositionInGrid { get; private set; }

        public SandPanelWidget Widget { get; set; }

        #endregion ISandPanelWidgetGridCell Members
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------