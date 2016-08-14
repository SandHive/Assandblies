using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Basics
{
    public class FhemClient : IDisposable
    {
        //---------------------------------------------------------------------
        #region Events

        /// <summary>
        /// Occurs when the 'IsConnected' property has changed.
        /// </summary>
        public event EventHandler IsConnectedChanged;

        //-- Events
        #endregion
        //---------------------------------------------------------------------
        #region Fields

        private bool  m_isConnected;

        private NetworkStream  m_networkStream;

        private TcpClient  m_tcpClient;

        //-- Fields
        #endregion
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets a flag that indicates whether this Fhem client is connected
        /// or disconnected.
        /// </summary>
        public bool IsConnected
        {
            get { return m_isConnected; }
            private set
            {
                //-- Do nothing when the value has not changed
                if( value == m_isConnected ) { return; }

                //-- Update the member variable
                m_isConnected = value;

                //-- Raise the corresponding event
                this.IsConnectedChanged?.Invoke( this, EventArgs.Empty );
            }
        }

        public int ReadBufferSize { get; set; } = 1024;

        public int ReadTimeoutInMs { get; set; } = 1000;

        //-- Properties
        #endregion
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FhemClient class.
        /// </summary>
        public FhemClient() { }

        /// <summary>
        /// Initializes a new instance of the FhemClient class.
        /// </summary>
        /// <param name="a_hostName">
        /// The host name of the Fhem server.
        /// </param>
        /// <param name="a_telnetPort">
        /// The Fhem server port where telnet is provided.
        /// </param>
        public FhemClient( string a_hostName, int a_telnetPort )
        {
            this.Connect( a_hostName, a_telnetPort );
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region IDisposable Members

        public void Dispose()
        {
            m_networkStream?.Dispose();
            m_tcpClient?.Close();
        }

        //-- IDisposable Members
        #endregion
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Connects this Fhem client with a Fhem server.
        /// </summary>
        /// <param name="a_hostName">
        /// The host name of the Fhem server.
        /// </param>
        /// <param name="a_telnetPort">
        /// The Fhem server port where telnet is provided.
        /// </param>
        public void Connect( string a_hostName, int a_telnetPort )
        {
            m_tcpClient = new TcpClient( a_hostName, a_telnetPort );

            m_networkStream = m_tcpClient.GetStream();

            this.IsConnected = true;
        }
              
        [DebuggerStepThrough]
        public string GetApplicationTime()
        {
            return this.SendNativeCommand( "apptime" );
        }

        [DebuggerStepThrough]
        public string GetXmlList()
        {
            return this.SendNativeCommand( "xmllist" );
        }

        /// <summary>
        /// Sends a native Fhem command.
        /// </summary>
        /// <param name="a_nativeCommandString">
        /// The native Fhem command.
        /// </param>
        /// <returns>
        /// The native Fhem response.
        /// </returns>
        public string SendNativeCommand( string a_nativeCommandString )
        {
            //-- Each FHEM command must end with "\r\n"
            if( !a_nativeCommandString.EndsWith( "\r\n" ) )
            {
                a_nativeCommandString = a_nativeCommandString + "\r\n";
            }

            var writeBuffer = Encoding.ASCII.GetBytes( a_nativeCommandString );

            m_networkStream.Write( writeBuffer, 0, writeBuffer.Length );
            m_networkStream.Flush();

            var readingDateTime = DateTime.Now;

            var readBuffer = new byte[this.ReadBufferSize];

            StringBuilder stringBuilder = null;

            do
            {
                if( m_networkStream.DataAvailable )
                {
                    var readBytesCount = m_networkStream.Read( readBuffer, 0, this.ReadBufferSize );

                    if( readBytesCount == this.ReadBufferSize )
                    {
                        if( stringBuilder == null )
                        {
                            stringBuilder = new StringBuilder();
                        }

                        string readBufferAsString = Encoding.ASCII.GetString( readBuffer );

                        stringBuilder.Append( readBufferAsString );

                        readingDateTime = DateTime.Now;
                    }
                    else
                    {
                        string readBufferAsString = Encoding.ASCII.GetString( readBuffer );

                        if( stringBuilder != null )
                        {
                            stringBuilder.Append( readBufferAsString );

                            return stringBuilder.ToString();
                        }

                        return readBufferAsString;
                    }                                                
                }
            }
            while( ( (TimeSpan) ( DateTime.Now - readingDateTime ) ).TotalMilliseconds <= this.ReadTimeoutInMs );

            return String.Empty;
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------