/* Copyright (c) 2013 - 2014 The Sand Hive Project
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

using NUnit.Framework;
using System;
//-----------------------------------------------------------------------------
namespace Sand.Controls.Tests
{
    [TestFixture, RequiresSTA]
    public class SandWidgetGridTests
    {
        //---------------------------------------------------------------------
        #region Tests

        [Test]
        [ExpectedException( typeof( NotSupportedException ), ExpectedMessage = "Only \"SandWidget\" objects can be added! Use \"AddWidget\" methods instead." )]
        public void AddItem_AddingNonWidgetTest()
        {
            SandWidgetGrid _sandWidgetGrid = new SandWidgetGrid()
            {
                ColumnCount = 10,
                RowCount = 10
            };

            _sandWidgetGrid.AddItem( new SandPanelItem() { Content = "Test", Name = "_1" } );
        }

        [Test]
        [ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "The cell is already occupied!" )]
        public void AddWidget_AddingWidgetToOccupiedSpecificCellTest()
        {
            //-- Create necessary objects
            var grid = new SandWidgetGrid() { ColumnCount = 10, RowCount = 10 };
            
            var widget1 = new SandWidget() { Content = "Test", Name = "_1" };
            grid.AddWidget( widget1, 2, 3 );
            Assert.AreEqual( widget1, grid.WidgetGridCells[2, 3].Widget );

            var widget2 = new SandWidget() { Content = "Test", Name = "_2" };
            grid.AddWidget( widget2, 2, 3 );
        }

        [Test]
        public void AddWidget_AddingWidgetToSpecificCellTest()
        {
            //-- Create necessary objects
            var grid = new SandWidgetGrid() { ColumnCount = 10, RowCount = 10 };
            var widget = new SandWidget() { Content = "Test", Name = "_1" };

            grid.AddWidget( widget, 2, 3 );
            Assert.AreEqual( widget, grid.WidgetGridCells[2, 3].Widget ); 
        }

        [Test]
        public void SandWidgetGrid_ValidConstructorTest()
        {
            SandWidgetGrid _sandWidgetGrid = new SandWidgetGrid();
        }

        #endregion Tests
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------