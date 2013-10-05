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
    public sealed class SandPanelWidgetGridCell : Border
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

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets a flag that indicates whether the cell contains currently a 
        /// widget or not. 
        /// </summary>
        public bool ContainsWidget { get { return this.Widget != null; } }

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
            private set { this.SetValue( SandPanelWidgetGridCell.IsWidgetOverProperty, value ); }
        }

        /// <summary>
        /// Gets the widget that is currently in the cell. 
        /// </summary>
        public SandPanelWidget Widget { get; internal set; }

        /// <summary>
        /// Gets the x position of the cell within the parent grid. 
        /// </summary>
        public int xPosInGrid { get; private set; }

        /// <summary>
        /// Gets the y position of the cell within the parent grid. 
        /// </summary>
        public int yPosInGrid { get; private set; }

        #endregion Properties
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandPanelWidgetGridCell class.
        /// </summary>
        /// <param name="size">
        /// The cell size.
        /// </param>
        internal SandPanelWidgetGridCell( Size size, int xPosInGrid, int yPosInGrid )
        {
            base.Width = size.Width;
            base.Height = size.Height;
            this.xPosInGrid = xPosInGrid;
            this.yPosInGrid = yPosInGrid;

            SandPanelWidgetGrid.SetZIndex( this, int.MinValue );
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Methods

        internal void OnWidgetDropped( SandPanelWidget widget )
        {
            //-- Reset the "Widget" property of the grid cell from which the widget was moved
            widget.CurrentWidgetGridCell.Widget = null;

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