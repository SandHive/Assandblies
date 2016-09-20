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
using System.ComponentModel;
using System.Windows.Data;
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

        /// <summary>
        /// Gets the command for connecting to the Fhem server.
        /// </summary>
        public DelegateCommand ConnectCommand { get; private set; }

        /// <summary>
        /// Gets or sets the native command string. 
        /// </summary>
        public string NativeCommandString { get; set; }

        /// <summary>
        /// Gets the repository of all Fhem objects.
        /// </summary>
        public ICollectionView FhemObjects { get; private set; }

        /// <summary>
        /// Gets the response of the native command string.
        /// </summary>
        public string FhemResponse
        {
            get { return m_fhemResponse; }
            private set
            {
                //-- Check that the value has really changed
                if( value == m_fhemResponse ) { return; }

                //-- Apply value
                m_fhemResponse = value;

                //-- Propagate the change
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the IP adress of the Fhem server.
        /// </summary>
        public string FhemServerIP { get; set; } = "192.168.178.50";

        /// <summary>
        /// Gets or sets the port of the Fhem server.
        /// </summary>
        public string FhemServerPort { get; set; } = "7072";

        /// <summary>
        /// Gets a flag that specifies whether a Fhem client is connected.
        /// </summary>
        public bool IsFhemClientConnected {  get { return m_fhemClient.IsConnected; } }

        /// <summary>
        /// Gets the command for sending a native command string.
        /// </summary>
        public DelegateCommand SendNativeCommandStringCommand { get; private set; }

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
            this.SendNativeCommandStringCommand = new DelegateCommand( () => this.SendNativeCommandStringCommandAction() );
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
            //-- Connect to the Fhem server
            m_fhemClient.Connect( this.FhemServerIP, int.Parse( this.FhemServerPort ) );

            //-- Get the Fhem object repository 
            var fhemObjectRepository = m_fhemClient.GetObjectRepository();

            //-- Use the Fhem object repository as source for the collection view 
            this.FhemObjects = CollectionViewSource.GetDefaultView( fhemObjectRepository );

            //-- Sort the Fhem objects by their names
            this.FhemObjects.SortDescriptions.Add( new SortDescription( "Name", ListSortDirection.Ascending ) );

            //-- Force a property update
            this.OnPropertyChanged( "FhemObjects" );
        }

        /// <summary>
        /// The action that should be performed when executing the 'SendNativeCommandStringCommand'.
        /// </summary>
        private void SendNativeCommandStringCommandAction()
        {
            this.FhemResponse = m_fhemClient.SendNativeCommand( this.NativeCommandString );
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------