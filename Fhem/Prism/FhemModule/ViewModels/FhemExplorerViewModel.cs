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