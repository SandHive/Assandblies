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