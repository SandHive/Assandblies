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

using Sand.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
//-----------------------------------------------------------------------------
namespace SandPanelPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        /// <summary>
        /// A simple trace listener for writing debug messages into a TextBox.
        /// </summary>
        private class ListBoxTraceListener : TraceListener
        {
            //-----------------------------------------------------------------
            #region Fields

            private ListBox _debugOutputListBox;

            #endregion Fields
            //-----------------------------------------------------------------
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the ListBoxTraceListener class.
            /// </summary>
            /// <param name="debugOutputListBox">
            /// The ListBox to which the debug messages should be added.
            /// </param>
            public ListBoxTraceListener( ListBox debugOutputListBox )
            {
                _debugOutputListBox = debugOutputListBox;
            }

            #endregion Constructors
            //-----------------------------------------------------------------
            #region TraceListener Members

            public override void Write( string message )
            {
                this.WriteLine( message );
            }

            public override void WriteLine( string message )
            {
                if( message.Contains( "Widget moving started" ) )
                {
                    _debugOutputListBox.Items.Clear();
                }

                //-- Take care that only one-row items are added. So let's check 
                //-- if a message consists of several lines and add these lines 
                //-- separately
                var messageRows = message.Split( new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );
                TextBlock messageTextBlock;
                foreach( var messageRow in messageRows )
                {
                    messageTextBlock = new TextBlock() { Text = messageRow };
                    _debugOutputListBox.Items.Add( messageTextBlock );
                    _debugOutputListBox.ScrollIntoView( messageTextBlock );
                }
            }

            #endregion TraceListener Members
            //-----------------------------------------------------------------
        }

        //---------------------------------------------------------------------
        #region Constants

        private readonly Cursor TARGET_CURSOR;

        private readonly Cursor TARGET_HOVERED_CURSOR;

        #endregion Constants
        //---------------------------------------------------------------------
        #region Fields

        private bool _isManualWidgetSelectingEnabled;

        private SandWidget _manualMovingWidget;

        #endregion Fields
        //---------------------------------------------------------------------
        #region Properties

        public static DependencyProperty AreManualWidgetMovingButtonsEnabledProperty = DependencyProperty.Register( "AreManualWidgetMovingButtonsEnabled", typeof( bool ), typeof( MainWindow ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets a flag that indicates whether the widget moving buttons are 
        /// enabled or not.
        /// </summary>
        public bool AreManualWidgetMovingButtonsEnabled
        {
            get { return (bool) this.GetValue( MainWindow.AreManualWidgetMovingButtonsEnabledProperty ); }
            private set { this.SetValue( MainWindow.AreManualWidgetMovingButtonsEnabledProperty, value ); }
        }

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

        public static DependencyProperty IsManualWidgetMovingEnabledProperty = DependencyProperty.Register( "IsManualWidgetMovingEnabled", typeof( bool ), typeof( MainWindow ), new PropertyMetadata( false ) );
        /// <summary>
        /// Gets a flag that indicates whether the manual widget moving is 
        /// currently enabled or not.
        /// </summary>
        public bool IsManualWidgetMovingEnabled
        {
            get { return (bool) this.GetValue( MainWindow.IsManualWidgetMovingEnabledProperty ); }
            private set { this.SetValue( MainWindow.IsManualWidgetMovingEnabledProperty, value ); }
        }

        public static DependencyProperty WidgetGuidProperty = DependencyProperty.Register( "WidgetGuid", typeof( String ), typeof( MainWindow ), new PropertyMetadata( String.Empty ) );
        /// <summary>
        /// Gets the guid of the widget that is dropped to the cell that is 
        /// currently hovered.
        /// </summary>
        public String WidgetGuid
        {
            get { return (String) this.GetValue( MainWindow.WidgetGuidProperty ); }
            private set { this.SetValue( MainWindow.WidgetGuidProperty, value ); }
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

            var executingAssembly = Assembly.GetExecutingAssembly();
            var executingAssemblyLocation = Path.GetDirectoryName( executingAssembly.Location );
            var imagesLocation = Path.Combine( executingAssemblyLocation, "Images" );

            TARGET_CURSOR = new Cursor( Path.Combine( imagesLocation, "Sniper Scope_Normal Select.ani" ) );
            TARGET_HOVERED_CURSOR = new Cursor( Path.Combine( imagesLocation, "Sniper Scope_Link Select.ani" ) );
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region Event Handling

        #region Widget Adding

        private void Add1x1WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 1, 1 )
                }
            );
        }

        private void Add1x2WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 1, 2 )
                }
            );
        }

        private void Add1x3WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 1, 3 )
                }
            );
        }

        private void Add2x1WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 2, 1 )
                }
            );
        }

        private void Add2x2WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 2, 2 )
                }
            );
        }

        private void Add2x3WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 2, 3 )
                }
            );
        }

        private void Add3x1WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 3, 1 )
                }
            );
        }

        private void Add3x2WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 3, 2 )
                }
            );
        }

        private void Add3x3WidgetButton_Click( object sender, RoutedEventArgs e )
        {
            _sandWidgetGrid.AddWidget(

                new SandWidget()
                {
                    Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) },
                    Name = "_" + _sandWidgetGrid.Children.Count.ToString(),
                    TileSize = new Size( 3, 3 )
                }
            );
        }

        #endregion Widget Adding

        #region Manual Widget Moving

        private void ManualWidgetMovingDownButton_Click( object sender, RoutedEventArgs e )
        {
            this.MoveWidgetRelativeToCurrentCell( 0, 1 );
        }

        private void ManualWidgetMovingLeftButton_Click( object sender, RoutedEventArgs e )
        {
            this.MoveWidgetRelativeToCurrentCell( -1, 0 );
        }

        private void ManualWidgetMovingRightButton_Click( object sender, RoutedEventArgs e )
        {
            this.MoveWidgetRelativeToCurrentCell( 1, 0 );
        }

        private void ManualWidgetMovingUpButton_Click( object sender, RoutedEventArgs e )
        {
            this.MoveWidgetRelativeToCurrentCell( 0, -1 );
        }

        #endregion Manual Widget Moving

        private void SelectWidgetToggleButton_Click( object sender, RoutedEventArgs e )
        {
            if( SelectWidgetToggleButton.IsChecked == true )
            {
                Mouse.OverrideCursor = TARGET_CURSOR;

                this.IsManualWidgetMovingEnabled = true;
                _isManualWidgetSelectingEnabled = true;

                SandWidgetMovement.Mode = SandWidgetMovementModes.Manual;
            }
            else
            {
                Mouse.OverrideCursor = null;

                this.IsManualWidgetMovingEnabled = false;
                _isManualWidgetSelectingEnabled = false;
            }
        }

        private void StopManualWidgetMovingButton_Click( object sender, RoutedEventArgs e )
        {
            this.StopManualWidgetMoving();
        }

        private void Window_KeyDown( object sender, KeyEventArgs e )
        {
            if( _manualMovingWidget == null ) { return; }

            switch( e.Key )
            {
                case Key.Down: this.MoveWidgetRelativeToCurrentCell( 0, 1 ); break;
                case Key.Left: this.MoveWidgetRelativeToCurrentCell( -1, 0 ); break;
                case Key.Right: this.MoveWidgetRelativeToCurrentCell( 1, 0 ); break;
                case Key.Up: this.MoveWidgetRelativeToCurrentCell( 0, -1 ); break;
                case Key.Escape: this.StopManualWidgetMoving(); break;
            }
        }

        private void Window_Loaded( object sender, RoutedEventArgs e )
        {
            Debug.Listeners.Add( new ListBoxTraceListener( _DebugOutputListBox ) );

            _sandWidgetGrid.AddWidget( new SandWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) }, Name = "_4" } );
            _sandWidgetGrid.AddWidget( new SandWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) }, Name = "_5" } );
            _sandWidgetGrid.AddWidget( new SandWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) }, Name = "_6" } );
            _sandWidgetGrid.AddWidget( new SandWidget() { Content = new Image() { Source = new BitmapImage( new Uri( @"pack://application:,,,/SandPanelPrototype;component/Images/convert_icon256.png" ) ) }, Name = "_7" }, 3, 3 );
        }

        private void Window_PreviewMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            //-- Do nothing when the manual moving was not enabled before
            if( !this.IsManualWidgetMovingEnabled ) { return; }

            //-- Determine the affected cell
            var mousePosition = e.GetPosition( _sandWidgetGrid );
            var cell = _sandWidgetGrid.GetCellRelativeToPoint( mousePosition );

            if( cell.ContainsWidget )
            {
                //-- Reset the TARGET cursor
                Mouse.OverrideCursor = null;

                //-- Mark the widget selcting as finished (otherwise the TARGET cursors will be set again)
                _isManualWidgetSelectingEnabled = false;

                //-- Keep the widget that will be manually moved in mind
                _manualMovingWidget = cell.Widget;
                _manualMovingWidget.StartMovement();

                //-- Apply the widget name to the content of the moving stopping button ...
                StopManualWidgetMovingButton.Content = String.Format( "Stop moving widget \"{0}\"", _manualMovingWidget.Name );
                //-- ... and make the button visible
                StopManualWidgetMovingButton.Visibility = System.Windows.Visibility.Visible;

                //-- Reset the checked state of the toggle button ...
                SelectWidgetToggleButton.IsChecked = false;
                //-- ... and collapse it
                SelectWidgetToggleButton.Visibility = System.Windows.Visibility.Collapsed;

                //-- Activate the "Up", "Down", "Right" and "Left" buttons
                this.AreManualWidgetMovingButtonsEnabled = true;
            }
        }

        private void Window_PreviewMouseMove( object sender, MouseEventArgs e )
        {
            var mousePosition = e.GetPosition( _sandWidgetGrid );
            var cell = _sandWidgetGrid.GetCellRelativeToPoint( mousePosition );

            //-- Show some cell and widget details
            this.CellGuid = cell.Guid;
            this.CellIndexes = new Point( cell.XCellIndex, cell.YCellIndex );

            if( cell.ContainsWidget )
            {
                var widget = cell.Widget;
                this.WidgetGuid = widget.Guid.ToString();
                this.WidgetName = widget.Name;

                if( _isManualWidgetSelectingEnabled )
                {
                    Mouse.OverrideCursor = TARGET_HOVERED_CURSOR;
                }
            }
            else
            {
                this.WidgetGuid = String.Empty;
                this.WidgetName = String.Empty;

                if( _isManualWidgetSelectingEnabled )
                {
                    Mouse.OverrideCursor = TARGET_CURSOR;
                }
            }
        }

        #endregion Event Handling
        //---------------------------------------------------------------------
        #region IDisposable

        public void Dispose()
        {
            TARGET_CURSOR.Dispose();
            TARGET_HOVERED_CURSOR.Dispose();
        }

        #endregion IDisposable
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Moves the selected widget relative to its current cell.
        /// </summary>
        /// <param name="xOffset">
        /// The offset by which the widget should be moved on the x axe.  
        /// </param>
        /// <param name="yOffset">
        /// The offset by which the widget should be moved on the y axe.
        /// </param>
        private void MoveWidgetRelativeToCurrentCell( int xOffset, int yOffset )
        {
            //-- No selected widget -> no moving!
            if( _manualMovingWidget == null ) { return; }

            //-- Create a shortcut for the current cell
            var currentCell = _manualMovingWidget.Movement.CurrentCell;

            #region var nextXIndex = ...

            //-- Determine the next x index in consideration of the grid borders
            var nextXIndex = currentCell.XCellIndex + xOffset;
            if( nextXIndex < 0 )
            {
                nextXIndex = 0;
            }
            else if( nextXIndex > _sandWidgetGrid.ColumnCount - 1 )
            {
                nextXIndex = _sandWidgetGrid.ColumnCount - 1;
            }

            #endregion var nextXIndex = ...
            #region var nextYIndex = ...

            //-- Determine the next y index in consideration of the grid borders
            var nextYIndex = currentCell.YCellIndex + yOffset;
            if( nextYIndex < 0 )
            {
                nextYIndex = 0;
            }
            else if( nextYIndex > _sandWidgetGrid.RowCount - 1 )
            {
                nextYIndex = _sandWidgetGrid.RowCount - 1;
            }

            #endregion var nextYIndex = ...
            var nextCell = _sandWidgetGrid.WidgetGridCells[nextXIndex, nextYIndex];

            //-- Move the widget to the new cell
            _manualMovingWidget.Movement.MoveWidgetTo( nextCell );

            //-- Place the widget to the new cell's center
            nextCell.OnWidgetDropped( _manualMovingWidget, false );
        }

        /// <summary>
        /// Stops the manual widget moving.
        /// </summary>
        private void StopManualWidgetMoving()
        {
            //-- Stop the manual widget moving
            _manualMovingWidget.StopMovement();
            _manualMovingWidget = null;

            //-- Reset the movement mode in order to enable the mouse controlling again
            SandWidgetMovement.Mode = SandWidgetMovementModes.DragAndDrop;

            //-- Reset all made settings again
            this.AreManualWidgetMovingButtonsEnabled = false;
            this.IsManualWidgetMovingEnabled = false;
            SelectWidgetToggleButton.Visibility = System.Windows.Visibility.Visible;
            StopManualWidgetMovingButton.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion Methods
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------