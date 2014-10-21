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
using System.Collections.Generic;
using System.Windows;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    [Serializable]
    public sealed class SandWidgetGridAllocation
    {
        //---------------------------------------------------------------------
        #region Fields

        private int  _columnCount;

        private int  _rowCount;

        #endregion Fields
		//---------------------------------------------------------------------
		#region Fields

		public IList<SandWidgetAllocationInfo> widgetsAllocationInfo { get; private set; }

		#endregion Fields
		//---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SandWidgetGridAllocation class.
        /// </summary>
        /// <param name="columnCount">
        /// The number of grid columns.
        /// </param>
        /// <param name="rowCount">
        /// The number of grid rows. 
        /// </param>
        /// <param name="widgetsAllocationInfo">
        /// The allocation info of all widgets that are currently added to the 
        /// grid.
        /// </param>
        private SandWidgetGridAllocation( int columnCount, int rowCount, IList<SandWidgetAllocationInfo> widgetsAllocationInfo )
        {
            //-- Apply arguments
            _columnCount = columnCount;
            _rowCount = rowCount;
            this.widgetsAllocationInfo = widgetsAllocationInfo;
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Creates the allocation object of a widget grid.
        /// </summary>
        /// <param name="columnCount">
        /// The number of grid columns.
        /// </param>
        /// <param name="rowCount">
        /// The number of grid rows.
        /// </param>
        /// <param name="widgetsAllocationInfo">
        /// The widget information that are needed for calculating the grid 
        /// allocation.
        /// </param>
        /// <exception cref="ArgumentException">
        /// At least two widgets overlap each other
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The widget is out of the grid bounds.
        /// </exception>
        /// <returns>
        /// The created SandWidgetGridAllocation object.
        /// </returns>
        public static SandWidgetGridAllocation Create( int columnCount, int rowCount, IList<SandWidgetAllocationInfo> widgetsAllocationInfo )
        {
            //-- Create a matrix for checking the occupation of the widget cells
            bool[,] cellsAllocationInfo = new bool[rowCount, columnCount];

            //-- Check that ...
            Point bottomRight; 
            foreach( var widgetInfo in widgetsAllocationInfo )
            {
                //-- ... all widgets fit to the column and row counts
                if( ( widgetInfo.TopLeft.X < 0 ) ||
                    ( widgetInfo.TopLeft.Y < 0 ) ||
                    ( ( widgetInfo.TopLeft.X + widgetInfo.TileSize.Width ) > columnCount ) ||
                    ( ( widgetInfo.TopLeft.Y + widgetInfo.TileSize.Height ) > rowCount ) )
                {
                    throw new ArgumentOutOfRangeException( String.Format( "The widget \"{0}\" is out of the grid bounds!", widgetInfo.WidgetName ) );
                }

                #region //-- ... no widget overlaps another one

                //-- Determine the widget's bottom right point 
                bottomRight = new Point( 
                    
                    widgetInfo.TopLeft.X + widgetInfo.TileSize.Width, 
                    widgetInfo.TopLeft.Y + widgetInfo.TileSize.Height 
                );

                for( int rowIndex = (int) widgetInfo.TopLeft.Y; rowIndex < bottomRight.Y; rowIndex++ )
                {
                    for( int columnIndex = (int) widgetInfo.TopLeft.X; columnIndex < bottomRight.X; columnIndex++ )
                    {
                        if( cellsAllocationInfo[rowIndex, columnIndex] == true )
                        {
                            throw new ArgumentException( "At least two widgets overlap each other!" );
                        }

                        //-- Mark the cell as occupied
                        cellsAllocationInfo[rowIndex, columnIndex] = true;
                    }
                }

                #endregion //-- ... no widget overlaps another one
            }

            return new SandWidgetGridAllocation( columnCount, rowCount, widgetsAllocationInfo );
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------
