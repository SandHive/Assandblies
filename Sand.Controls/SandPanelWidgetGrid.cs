﻿/* Copyright (c) 2013 The Sandmakers
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
using System.Windows.Media;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public class SandPanelWidgetGrid : SandPanel
    {
        //---------------------------------------------------------------------
        #region Properties

        public static DependencyProperty CellHeightProperty = DependencyProperty.Register( "CellHeight", typeof( double ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( 50.0 ) );
        /// <summary>
        /// Gets or sets the height of a cell.
        /// </summary>
        public double CellHeight
        {
            get { return (double) this.GetValue( SandPanelWidgetGrid.CellHeightProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellHeightProperty, value ); }
        }

        public static DependencyProperty CellMarginProperty = DependencyProperty.Register( "CellMargin", typeof( Thickness ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Thickness( 0 ) ) );
        /// <summary>
        /// Gets or sets the cell margin.
        /// </summary>
        public Thickness CellMargin
        {
            get { return (Thickness) this.GetValue( SandPanelWidgetGrid.CellMarginProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellMarginProperty, value ); }
        }

        public static DependencyProperty CellPaddingProperty = DependencyProperty.Register( "CellPadding", typeof( Thickness ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( new Thickness( 10 ) ) );
        /// <summary>
        /// Gets or sets the cell padding.
        /// </summary>
        public Thickness CellPadding
        {
            get { return (Thickness) this.GetValue( SandPanelWidgetGrid.CellPaddingProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellPaddingProperty, value ); }
        }

        public static DependencyProperty CellWidthProperty = DependencyProperty.Register( "CellWidth", typeof( double ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( 50.0 ) );
        /// <summary>
        /// Gets or sets the width of a cell.
        /// </summary>
        public double CellWidth
        {
            get { return (double) this.GetValue( SandPanelWidgetGrid.CellWidthProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.CellWidthProperty, value ); }
        }

        public static DependencyProperty ColumnCountProperty = DependencyProperty.Register( "ColumnCount", typeof( int ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( 4 ) );
        /// <summary>
        /// Gets or sets the number of columns.
        /// </summary>
        public int ColumnCount
        {
            get { return (int) this.GetValue( SandPanelWidgetGrid.ColumnCountProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.ColumnCountProperty, value ); }
        }

        public static DependencyProperty RowCountProperty = DependencyProperty.Register( "RowCount", typeof( int ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( 4 ) );
        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        public int RowCount
        {
            get { return (int) this.GetValue( SandPanelWidgetGrid.RowCountProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.RowCountProperty, value ); }
        }

        public static DependencyProperty ShowGridProperty = DependencyProperty.Register( "ShowGrid", typeof( bool ), typeof( SandPanelWidgetGrid ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets or sets a flag whether the grid should be shown or not.
        /// </summary>
        public bool ShowGrid
        {
            get { return (bool) this.GetValue( SandPanelWidgetGrid.ShowGridProperty ); }
            set { this.SetValue( SandPanelWidgetGrid.ShowGridProperty, value ); }
        }
        
        #endregion Properties
        //---------------------------------------------------------------------
        #region SandPanel Members

        protected override void OnInitialized( EventArgs e )
        {
            //-- Call the base implementation
            base.OnInitialized( e );

            //-- Calculate the total size of a cell
            Size totalCellSize = new Size(

                this.CellWidth + this.CellPadding.Left + this.CellPadding.Right,
                this.CellHeight + this.CellPadding.Top + this.CellPadding.Bottom
            );

            //-- Adjust the panel height and width
            base.Height = this.RowCount * totalCellSize.Height;
            base.Width = this.ColumnCount * totalCellSize.Width;

            #region //-- Add all cells

            SandPanelWidgetGridCell cell;
            double cellLeft = 0;
            double cellTop = 0;
            for( int i = 0; i < this.RowCount; i++ )
            {
                cellLeft = 0;

                for( int j = 0; j < this.ColumnCount; j++ )
                {
                    cell = new SandPanelWidgetGridCell( totalCellSize );
                    
                    if( this.ShowGrid )
                    { 
                        cell.BorderBrush = Brushes.Black;
                        cell.BorderThickness = new Thickness( 1 );
                    };

                    base.Children.Add( cell );
                    SandPanelWidgetGrid.SetLeft( cell, cellLeft );
                    SandPanelWidgetGrid.SetTop( cell, cellTop );

                    cellLeft += totalCellSize.Width;
                }

                cellTop += totalCellSize.Height;
            }

            #endregion //-- Add all cells
        }

        #endregion SandPanel Members
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------