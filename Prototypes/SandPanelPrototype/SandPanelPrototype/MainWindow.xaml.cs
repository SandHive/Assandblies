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

using Sand.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        #region Properties

        public static DependencyProperty CellGuidProperty = DependencyProperty.Register( "CellGuid", typeof( Guid ), typeof( MainWindow ), new PropertyMetadata( Guid.Empty ) );
        /// <summary>
        /// Gets the guid of the cell that is currently hovered.
        /// </summary>
        public Guid CellGuid
        {
            get { return (Guid) this.GetValue( MainWindow.CellGuidProperty ); }
            private set { this.SetValue( MainWindow.CellGuidProperty, value ); }
        }

        public static DependencyProperty CellIndexesProperty = DependencyProperty.Register( "CellIndexes", typeof( Point ), typeof( MainWindow ), new PropertyMetadata( new Point() ) );
        /// <summary>
        /// Gets the indexes of the cell that is currently hovered.
        /// </summary>
        public Point CellIndexes
        {
            get { return (Point) this.GetValue( MainWindow.CellIndexesProperty ); }
            private set { this.SetValue( MainWindow.CellIndexesProperty, value ); }
        }

        public static DependencyProperty WidgetGuidProperty = DependencyProperty.Register( "WidgetGuid", typeof( Guid ), typeof( MainWindow ), new PropertyMetadata( Guid.Empty ) );
        /// <summary>
        /// Gets the guid of the widget that is dropped to the cell that is 
        /// currently hovered.
        /// </summary>
        public Guid WidgetGuid
        {
            get { return (Guid) this.GetValue( MainWindow.WidgetGuidProperty ); }
            private set { this.SetValue( MainWindow.WidgetGuidProperty, value ); }
        }

        public static DependencyProperty WidgetHomeCellIndexesProperty = DependencyProperty.Register( "WidgetHomeCellIndexes", typeof( Point ), typeof( MainWindow ), new PropertyMetadata( new Point() ) );
        /// <summary>
        /// Gets the indexes of the home cell of the widget that is dropped to 
        /// the cell that is currently hovered.
        /// </summary>
        public Point WidgetHomeCellIndexes
        {
            get { return (Point) this.GetValue( MainWindow.WidgetHomeCellIndexesProperty ); }
            private set { this.SetValue( MainWindow.WidgetHomeCellIndexesProperty, value ); }
        }

        public static DependencyProperty WidgetNameProperty = DependencyProperty.Register( "WidgetName", typeof( String ), typeof( MainWindow ), new PropertyMetadata( String.Empty ) );
        /// <summary>
        /// Gets the name of the widget that is dropped to the cell that is 
        /// currently hovered.
        /// </summary>
        public String WidgetName
        {
            get { return (String) this.GetValue( MainWindow.WidgetNameProperty ); }
            private set { this.SetValue( MainWindow.WidgetNameProperty, value ); }
        }

        #endregion Properties
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
            _sandPanelWidgetGrid.AddItem( new SandPanelWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) }, Name = "_4" } );
            _sandPanelWidgetGrid.AddItem( new SandPanelWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) }, Name = "_5" } );
            _sandPanelWidgetGrid.AddItem( new SandPanelWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) }, Name = "_6" } );
        }

        private void Window_PreviewMouseMove( object sender, MouseEventArgs e )
        {
            var mousePosition = e.GetPosition( _sandPanelWidgetGrid );
            var cell = _sandPanelWidgetGrid.GetCellRelativeToPoint( mousePosition );

            //-- Show some cell and widget details
            this.CellGuid = cell.Guid;
            this.CellIndexes = new Point( cell.XCellIndex, cell.YCellIndex );

            if( cell.ContainsWidget )
            {
                var widget = cell.Widget;
                this.WidgetGuid = widget.Guid;
                this.WidgetHomeCellIndexes = new Point( widget.HomeWidgetGridCell.XCellIndex, widget.HomeWidgetGridCell.XCellIndex );
                this.WidgetName = widget.Name;
            }
            else
            {
                this.WidgetGuid = Guid.Empty;
                this.WidgetHomeCellIndexes = new Point();
                this.WidgetName = String.Empty;
            }           
        }
        
        #endregion Event Handling
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------