using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        private NetworkStreamReader  m_networkStreamReader;

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
            //-- Clean up everything
            m_networkStreamReader?.Dispose();
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
            //-- Create the TCP client
            m_tcpClient = new TcpClient( a_hostName, a_telnetPort );

            //-- Create all other necessary objects
            m_networkStream = m_tcpClient.GetStream();
            m_networkStreamReader = new NetworkStreamReader( m_networkStream );

            //-- Set a flag that we are connected
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
        /// <exception cref="ArgumentNullException">
        /// The native command string may not be null or empty.
        /// </exception>
        /// <returns>
        /// The native Fhem response.
        /// </returns>
        public string SendNativeCommand( string a_nativeCommandString )
        {
            if( String.IsNullOrWhiteSpace( a_nativeCommandString ) )
            {
                throw new ArgumentNullException( "The native command string may not be null or empty!" );
            }

            //-- Each FHEM command must end with "\r\n"
            if( !a_nativeCommandString.EndsWith( "\r\n" ) )
            {
                a_nativeCommandString = a_nativeCommandString + "\r\n";
            }
            
            //-- Convert the native string into a byte array
            var writeBuffer = Encoding.ASCII.GetBytes( a_nativeCommandString );

            //-- Send the byte array
            m_networkStream.Write( writeBuffer, 0, writeBuffer.Length );
            m_networkStream.Flush();

            var result = m_networkStreamReader.ReadString();
                                    
            return result;
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------