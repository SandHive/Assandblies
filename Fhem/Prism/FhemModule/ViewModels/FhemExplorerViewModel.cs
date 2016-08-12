using Prism.Commands;
using Prism.Mvvm;
using Sand.Fhem.Basics;
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Prism.FhemModule.ViewModels
{
    public class FhemExplorerViewModel : BindableBase
    {
        //---------------------------------------------------------------------
        #region Fields

        private FhemClient  m_fhemClient;

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

        public DelegateCommand SendCommand { get; private set; }

        //-- Properties
        #endregion
        //---------------------------------------------------------------------
        #region Constructors

        public FhemExplorerViewModel()
        {
            //-- Initialize commands
            this.ConnectCommand = new DelegateCommand( () => this.ConnectCommandAction()  );
            this.SendCommand = new DelegateCommand( () => this.SendCommandAction() );
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region Methods

        private void ConnectCommandAction()
        {
            m_fhemClient = new FhemClient( this.FhemServerIP, int.Parse( this.FhemServerPort ) );
        }

        private void SendCommandAction()
        {
            this.FhemResponse = m_fhemClient.SendCommand( this.FhemCommand );
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------