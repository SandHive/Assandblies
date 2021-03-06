﻿/* Copyright (c) 2013 - 2014 The Sandhive Project (http://sandhive.org)
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
    public class SandWidgetMovementTests
    {
        //---------------------------------------------------------------------
        #region Tests

        [Test]
        [ExpectedException( typeof( ArgumentNullException ), ExpectedMessage = "The home cell may not be null!" )]
        public void Start_HomeCellArgumentNullTest()
        {
            //-- Create necessary objects
            var grid = new SandWidgetGrid();
            var widget = new SandWidget() { Content = "Test", Name = "_1" }; grid.AddWidget( widget );

            SandWidgetMovement.Start( widget, null );
        }

        [Test]
        [ExpectedException( typeof( ArgumentNullException ), ExpectedMessage = "The widget may not be null!" )]
        public void Start_WidgetArgumentNullTest()
        {
            SandWidgetMovement.Start( null, null );
        }

        [Test]
        public void MoveWidgetTo_MoveToEmptyNeighborCellTest()
        {
            //-- Create necessary objects
            var grid = new SandWidgetGrid();
            var widget = new SandTestWidget() { Content = "Test", Name = "_1" }; grid.AddWidget( widget, 0, 0 );
            var homeCell = grid.WidgetGridCells[0, 0];
            var neighborCell = grid.WidgetGridCells[0, 1];

            var movement = SandWidgetMovement.Start( widget, homeCell );

            //-- Check the cell's widget before calling the "MoveWidgetTo" method ...
            Assert.AreEqual( widget, homeCell.WidgetHost );
            Assert.AreEqual( null, neighborCell.WidgetHost );

            movement.MoveWidgetTo( neighborCell );

            //-- ... and afterwards
            Assert.AreEqual( null, homeCell.WidgetHost );
            Assert.AreEqual( widget, neighborCell.WidgetHost );
        }

        [Test]
        public void MoveWidgetTo_MoveToOccupiedNeighborCellsTest()
        {
            //-- Create necessary objects
            var grid = new SandWidgetGrid();
            var movingWidget = new SandWidget() { Content = "movingWidget", Name = "movingWidget" }; grid.AddWidget( movingWidget, 0, 0 );
            var neighborWidget1 = new SandWidget() { Content = "neighborWidget1", Name = "neighborWidget1" }; grid.AddWidget( neighborWidget1, 0, 1 );
            var neighborWidget2 = new SandWidget() { Content = "neighborWidget2", Name = "neighborWidget2" }; grid.AddWidget( neighborWidget2, 0, 2 );
            var homeCell = grid.WidgetGridCells[0, 0];
            var neighborCell1 = grid.WidgetGridCells[0, 1];
            var neighborCell2 = grid.WidgetGridCells[0, 2];

            //-- Check the cell's widget before executing the first "MoveWidgetTo" call ...
            var movement = SandWidgetMovement.Start( movingWidget, homeCell );
            Assert.AreEqual( movingWidget, homeCell.WidgetHost );
            Assert.AreEqual( neighborWidget1, neighborCell1.WidgetHost );
            Assert.AreEqual( neighborWidget2, neighborCell2.WidgetHost );

            //-- ... and afterwards ...
            movement.MoveWidgetTo( neighborCell1 );
            Assert.AreEqual( neighborWidget1, homeCell.WidgetHost );
            Assert.AreEqual( movingWidget, neighborCell1.WidgetHost );
            Assert.AreEqual( neighborWidget2, neighborCell2.WidgetHost );

            //-- ... and after the second "MoveWidgetTo" call
            movement.MoveWidgetTo( neighborCell2 );
            //-- Notice: When moving a second time, the first swapped widget (in this case "neighborWidget1")
            //-- should be moved back to its own home cell which is not occupied any more by the moving widget
            Assert.AreEqual( neighborWidget2, homeCell.WidgetHost );
            Assert.AreEqual( neighborWidget1, neighborCell1.WidgetHost );
            Assert.AreEqual( movingWidget, neighborCell2.WidgetHost );
        }
        
        [Test]
        public void StartStop_ValidTest()
        {
            //-- Create necessary objects
            var grid = new SandWidgetGrid();
            var widget = new SandWidget() { Content = "Test", Name = "_1" }; grid.AddWidget( widget );
            var homeCell = grid.WidgetGridCells[0, 0];

            var widgetMovement = SandWidgetMovement.Start( widget, homeCell );
            Assert.AreEqual( true, homeCell.IsHome, "The cell that was set at \"SandWidgetMovement.Start\" should be marked as home cell!" );

            widgetMovement.Stop();
            Assert.AreEqual( false, homeCell.IsHome, "When stopping a widget movement, the home cell mark should be removed!" );
        }

        #endregion Tests
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------