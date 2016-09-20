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
using Newtonsoft.Json.Linq;
using System;
using System.Net.Sockets;
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

        private NetworkStreamReader  m_networkStreamReader;

        private NetworkStreamWriter  m_networkStreamWriter;

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

            //-- Get the network stream and create the reader and writer instances
            var networkStream = m_tcpClient.GetStream();
            m_networkStreamReader = new NetworkStreamReader( networkStream );
            m_networkStreamWriter = new NetworkStreamWriter( networkStream );

            //-- Set a flag that we are connected
            this.IsConnected = true;
        }

        /// <summary>
        /// Gets a repository with all available Fhem objects.
        /// </summary>
        /// <returns>
        /// A repository with all available Fhem objects.
        /// </returns>
        public FhemObjectRepository GetObjectRepository()
        {
            //-- Use the 'jsonlist2' command for creating the FHEM object repository
            var response = this.SendNativeCommand( "jsonlist2" );

            //-- Parse the response into a JSON object
            var jsonObject = JObject.Parse( response );

            //-- Create the FHEM object repository
            var fhemObjectRepository = FhemObjectRepository.Create( jsonObject );

            return fhemObjectRepository;
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

            //-- Write the native command string to the network stream
            m_networkStreamWriter.Write( a_nativeCommandString );

            //-- Read the response from the network stream
            var response = m_networkStreamReader.ReadString();

            return response;
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------