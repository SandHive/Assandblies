/* Copyright (c) 2016 The Sandman (sandhive@gmail.com)
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
using Prism.Commands;
using Prism.Mvvm;
using Sand.Fhem.Basics;
using System;
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Prism.FhemModule.ViewModels
{
    public class FhemExplorerViewModel : BindableBase
    {
        //---------------------------------------------------------------------
        #region Fields

        private FhemClient  m_fhemClient = new FhemClient();

        private string  m_fhemResponse;

        //-- Fields
        #endregion
        //---------------------------------------------------------------------
        #region Properties

        public DelegateCommand ConnectCommand { get; private set; }

        public string FhemCommand { get; set; }

        public FhemObjectRepository FhemObjects { get; private set; }

        public string FhemResponse
        {
            get { return m_fhemResponse; }
            set
            {
                if( value == m_fhemResponse ) { return; }

                m_fhemResponse = value;

                this.OnPropertyChanged();
            }
        }

        public string FhemServerIP { get; set; } = "192.168.178.50";

        public string FhemServerPort { get; set; } = "7072";

        public bool IsFhemClientConnected {  get { return m_fhemClient.IsConnected; } }

        public DelegateCommand SendCommand { get; private set; }

        //-- Properties
        #endregion
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FhemExplorerViewModel class.
        /// </summary>
        public FhemExplorerViewModel()
        {
            //-- Register to events
            m_fhemClient.IsConnectedChanged += FhemClient_IsConnectedChanged ;

            //-- Initialize commands
            this.ConnectCommand = new DelegateCommand( () => this.ConnectCommandAction()  );
            this.SendCommand = new DelegateCommand( () => this.SendCommandAction() );
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region Event Handlers

        private void FhemClient_IsConnectedChanged( object sender, EventArgs e )
        {
            //-- Just force an update of the 'IsFhemClientConnected' property
            this.OnPropertyChanged( "IsFhemClientConnected" );
        }

        //-- Event Handlers
        #endregion
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// The action that should be performed when executing the 'ConnectCommand'.
        /// </summary>
        private void ConnectCommandAction()
        {
            m_fhemClient.Connect( this.FhemServerIP, int.Parse( this.FhemServerPort ) );

            this.FhemObjects = m_fhemClient.GetObjectRepository();

            this.OnPropertyChanged( "FhemObjects" );
        }

        /// <summary>
        /// The action that should be performed when executing the 'SendCommand'.
        /// </summary>
        private void SendCommandAction()
        {
            this.FhemResponse = m_fhemClient.SendNativeCommand( this.FhemCommand );
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------