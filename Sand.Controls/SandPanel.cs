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

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//-----------------------------------------------------------------------------
namespace Sand.Controls
{
    public class SandPanel : Canvas
    {
        //---------------------------------------------------------------------
        #region Canvas Members

        protected override void OnInitialized( EventArgs e )
        {
            //-- Call the base implementation
            base.OnInitialized( e );

            //-- Handle the items that were added in xaml
            foreach( UIElement child in this.Children )
            {
                if( !( child is SandPanelItem ) ) { continue; }

                this.OnItemAdded( (SandPanelItem) child );
            }
        }

        #endregion Canvas Members
        //---------------------------------------------------------------------
        #region Event Handling
        
        private void item_MouseDown( object sender, MouseButtonEventArgs e )
        {
            //-- Create a shortcut to the SandPanelItem object
            var item = (SandPanelItem) sender;

            #region //-- Handle the ZIndex

            //-- Set the zindex of the current moved item to the possible maximum ...
            SandPanel.SetZIndex( item, this.Children.Count );

            foreach( UIElement child in this.Children )
            {
                if( !( child is SandPanelItem ) ) { continue; }
                if( item == child ) { continue; }

                //-- ... and decrease the zindex of all other children by 1
                SandPanel.SetZIndex( child, SandPanel.GetZIndex( child ) - 1 );
            }

            #endregion //-- Handle the ZIndex
        }

        #endregion Event Handling
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Adds an item to the SandPanel object.
        /// </summary>
        /// <param name="item">
        /// The SandPanelItem object.
        /// </param>
        public virtual void AddItem( SandPanelItem item )
        {
            //-- Add the item to the children collection
            this.Children.Add( item );

            this.OnItemAdded( item );
        }

        /// <summary>
        /// This method is invoked whenever a SandPanelItem object is added.
        /// </summary>
        /// <param name="item">
        /// The SandPanelItem object.
        /// </param>
        protected virtual void OnItemAdded( SandPanelItem item )
        {
            //-- Take care of the z index
            SandPanel.SetZIndex( item, this.Children.Count );

            //-- Register to events
            item.MouseDown += item_MouseDown;
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------