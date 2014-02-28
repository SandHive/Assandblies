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
    public class SandWidgetPositionerTests
    {
        //---------------------------------------------------------------------
        #region Tests

        [Test]
        public void ValidateWidgetMovement_CurrentCellEqualsNewHoveredCellTest()
        {
            //-- Create necessary objects
            var grid = new SandWidgetGrid();
            var widget = new SandWidget() { Content = "Test", Name = "_1" }; grid.AddWidget( widget );
            var cell = grid.WidgetGridCells[0, 0];
            var movement = new SandWidgetMovement( widget, cell );

            //-- Check the cell's widget before calling the validation method ...
            Assert.AreEqual( widget, cell.Widget );

            SandWidgetPositioner.ValidateWidgetMovement( movement, cell );

            //-- ... and afterwards
            Assert.AreEqual( widget, cell.Widget );
        }

        [Test]
        public void ValidateWidgetMovement_MoveWidgetToNeighborCellTest()
        {
            //-- Create necessary objects
            var grid = new SandWidgetGrid();
            var widget = new SandWidget() { Content = "Test", Name = "_1" }; grid.AddWidget( widget, 0, 0 );
            var cell = grid.WidgetGridCells[0, 0];
            var neighborCell = grid.WidgetGridCells[0, 1];
            var movement = new SandWidgetMovement( widget, cell );
            
            //-- Check the cell's widget before calling the validation method ...
            Assert.AreEqual( widget, cell.Widget );
            Assert.AreEqual( null, neighborCell.Widget );
            
            SandWidgetPositioner.ValidateWidgetMovement( movement, neighborCell );

            //-- ... and afterwards
            Assert.AreEqual( null, cell.Widget );
            Assert.AreEqual( widget, neighborCell.Widget );
        }
        
        [Test]
        [ExpectedException( typeof( ArgumentNullException ), ExpectedMessage = "The new hovered cell may not be null!", MatchType = MessageMatch.StartsWith )]
        public void ValidateWidgetMovement_NewHoveredCellArgumentNullTest()
        {
            SandWidgetPositioner.ValidateWidgetMovement( new SandWidgetMovement( null, null ), null );
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ), ExpectedMessage = "The widget movement may not be null!", MatchType = MessageMatch.StartsWith )]
        public void ValidateWidgetMovement_WidgetMovementArgumentNullTest()
        {
            SandWidgetPositioner.ValidateWidgetMovement( null, new SandWidgetGridCell( 0, 0 ) );
        }

        #endregion Tests
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------