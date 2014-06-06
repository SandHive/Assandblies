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
    public sealed class SandWidgetGridCell : SandWidgetGridCellBase
    {
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
        #region SandWidgetGridCellBase Members

        public override bool ContainsWidget
        {
            get { return this.Widget != null; }
        }

        public static DependencyProperty IsHomeProperty = DependencyProperty.Register( "IsHome", typeof( bool ), typeof( SandWidgetGridCell ), new PropertyMetadata( false ) );
        public override bool IsHome
        {
            get { return (bool) this.GetValue( SandWidgetGridCell.IsHomeProperty ); }
            internal set { this.SetValue( SandWidgetGridCell.IsHomeProperty, value ); }
        }

        public static DependencyProperty IsHoveredProperty = DependencyProperty.Register( "IsHovered", typeof( bool ), typeof( SandWidgetGridCell ), new PropertyMetadata( false ) );
        public override bool IsHovered
        {
            get { return (bool) this.GetValue( SandWidgetGridCell.IsHoveredProperty ); }
            internal set { this.SetValue( SandWidgetGridCell.IsHoveredProperty, value ); }
        }

        internal override void SetWidget( SandWidget widget, bool shouldWidgetBeCentered )
        {
            if( widget != null )
            {
                if( this.Widget != null )
                    throw new InvalidOperationException( "The cell is already occupied!" );

                if( shouldWidgetBeCentered )
                {
                    //-- Get the location of the cell within the widget grid
                    Point cellInGridLocation = ( (SandWidgetGrid) this.Parent ).GetCellLocation( this );

                    //-- Calculate the offset in order to center the widget
                    double xOffset = ( this.Width - widget.Width ) / 2;
                    double yOffset = ( this.Height - widget.Height ) / 2;

                    //-- Move the widget to the cell's center
                    SandWidgetGrid.SetLeft( widget, cellInGridLocation.X + xOffset );
                    SandWidgetGrid.SetTop( widget, cellInGridLocation.Y + yOffset );
                }
            }

            this.Widget = widget;
        }

        public override SandWidget Widget { get; protected set; }

        #endregion SandWidgetGridCellBase Members
        //---------------------------------------------------------------------
        #region Methods

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