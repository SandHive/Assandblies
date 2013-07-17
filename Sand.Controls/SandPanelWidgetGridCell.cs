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
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public class SandPanelWidgetGridCell : Border
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
        #region Properties

        public static DependencyProperty IsWidgetOverProperty = DependencyProperty.Register( "IsWidgetOver", typeof( bool ), typeof( SandPanelWidgetGridCell ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets or sets the height of a cell.
        /// </summary>
        public bool IsWidgetOver
        {
            get { return (bool) this.GetValue( SandPanelWidgetGridCell.IsWidgetOverProperty ); }
            private set { this.SetValue( SandPanelWidgetGridCell.IsWidgetOverProperty, value ); }
        }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandPanelWidgetGridCell class.
        /// </summary>
        /// <param name="size">
        /// The cell size.
        /// </param>
        public SandPanelWidgetGridCell( Size size )
        {
            base.Width = size.Width;
            base.Height = size.Height;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Methods

        internal void OnWidgetDropped( SandPanelWidget widget )
        {
            this.IsWidgetOver = false;

            #region //-- Place the widget to the cell's center

            //-- Get the location of the cell within the widget grid
            Point cellInGridLocation = ( (SandPanelWidgetGrid) this.Parent ).TranslatePoint( new Point(), this );

            //-- Calculate the offset in order to center the widget
            double xOffset = ( this.Width - widget.RenderSize.Width ) / 2;
            double yOffset = ( this.Height - widget.RenderSize.Height ) / 2;

            //-- Move the widget to the cell's center
            SandPanelWidgetGrid.SetLeft( widget, Math.Abs( cellInGridLocation.X ) + xOffset );
            SandPanelWidgetGrid.SetTop( widget, Math.Abs( cellInGridLocation.Y ) + yOffset );

            #endregion //-- Place the widget to the cell's center
        }

        internal void OnWidgetEnter( SandPanelWidget widget )
        {
            this.IsWidgetOver = true;
            this.RaiseEvent( new RoutedEventArgs( SandPanelWidgetGridCell.WidgetEnterEvent ) );
        }

        internal void OnWidgetLeave( SandPanelWidget widget )
        {
            this.IsWidgetOver = false;
            this.RaiseEvent( new RoutedEventArgs( SandPanelWidgetGridCell.WidgetLeaveEvent ) );
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------