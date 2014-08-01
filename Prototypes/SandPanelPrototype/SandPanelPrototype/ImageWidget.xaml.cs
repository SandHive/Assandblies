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
using System.Windows;
using System.Windows.Controls;
//-----------------------------------------------------------------------------
namespace SandPanelPrototype
{
    /// <summary>
    /// Interaction logic for ImageWidget.xaml
    /// </summary>
    public partial class ImageWidget : UserControl, ISandWidget
    {
        //---------------------------------------------------------------------
        #region Constructors

        public ImageWidget()
        {
            InitializeComponent();
        }

        #endregion Constructors
        //---------------------------------------------------------------------
        #region ISandWidget Members

        public static DependencyProperty TileSizeProperty = DependencyProperty.Register( "TileSize", typeof( Size ), typeof( ImageWidget ), new PropertyMetadata( new Size( 1.0, 1.0 ) ) );
        public Size TileSize
        {
            get { return (Size) this.GetValue( ImageWidget.TileSizeProperty ); }
            set { this.SetValue( ImageWidget.TileSizeProperty, value ); }
        }

        public FrameworkElement WidgetFrameworkElement
        {
            get { return this; }
        }

        #endregion ISandWidget Members
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------
