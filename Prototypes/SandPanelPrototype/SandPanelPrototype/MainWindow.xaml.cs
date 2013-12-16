﻿/* Copyright (c) 2013 The Sand Hive Project
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

using Sand.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//-----------------------------------------------------------------------------
namespace Prototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //---------------------------------------------------------------------
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Event Handling

        private void Add1x1WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem( 
                
                new SandPanelWidget() 
                { 
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) }, 
                    TileSize = new Size( 1, 1 )
                } 
            );
        }

        private void Add1x2WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem(

                new SandPanelWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    TileSize = new Size( 1, 2 )
                }
            );
        }

        private void Add1x3WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem(

                new SandPanelWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    TileSize = new Size( 1, 3 )
                }
            );
        }

        private void Add2x1WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem(

                new SandPanelWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    TileSize = new Size( 2, 1 )
                }
            );
        }

        private void Add2x2WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem(

                new SandPanelWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    TileSize = new Size( 2, 2 )
                }
            );
        }

        private void Add2x3WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem(

                new SandPanelWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    TileSize = new Size( 2, 3 )
                }
            );
        }

        private void Add3x1WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem(

                new SandPanelWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    TileSize = new Size( 3, 1 )
                }
            );
        }

        private void Add3x2WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem(

                new SandPanelWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    TileSize = new Size( 3, 2 )
                }
            );
        }

        private void Add3x3WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem(

                new SandPanelWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    TileSize = new Size( 3, 3 )
                }
            );
        }

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            _sandPanelWidgetGrid.AddItem( new SandPanelWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) } } );
            _sandPanelWidgetGrid.AddItem( new SandPanelWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) } } );
            _sandPanelWidgetGrid.AddItem( new SandPanelWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) } } );
        }
        
        #endregion Event Handling
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------