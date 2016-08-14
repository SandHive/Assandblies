/* Copyright (c) 2016 The Sandhive Project (http://sandhive.org)
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

using Prism.Modularity;
using Prism.Unity;
using System.Windows;
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Prism.FhemAgent
{
    internal class FhemBootstrapper : UnityBootstrapper
    {
        //---------------------------------------------------------------------
        #region UnityBootstrapper Members

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            this.ModuleCatalog.AddModule( new ModuleInfo( "FhemModule", "Sand.Fhem.Prism.FhemModule.FhemModule, Sand.Fhem.Prism.FhemModule" ) );
        }

        protected override DependencyObject CreateShell()
        {
            return new FhemShell();
        }

        protected override void InitializeShell()
        {
            FhemApplication.Current.MainWindow = (Window) this.Shell;
            FhemApplication.Current.MainWindow.Show();
        }

        //-- UnityBootstrapper Members
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------