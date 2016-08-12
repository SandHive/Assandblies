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
        #region Fields

        private TcpClient  m_tcpClient;

        //-- Fields
        #endregion
        //---------------------------------------------------------------------
        #region Properties

        public int ReadBufferSize { get; set; } = 1024;

        public int ReadTimeoutInMs { get; set; } = 1000;

        //-- Properties
        #endregion
        //---------------------------------------------------------------------
        #region Constructors

        public FhemClient( string a_hostName, int a_port )
        {
            m_tcpClient = new TcpClient( a_hostName, a_port );
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region IDisposable Members

        public void Dispose()
        {
            m_tcpClient?.Close();
        }

        //-- IDisposable Members
        #endregion
        //---------------------------------------------------------------------
        #region Methods
            
        [DebuggerStepThrough]
        public string GetApplicationTime()
        {
            return this.SendCommand( "apptime" );
        }

        [DebuggerStepThrough]
        public string GetXmlList()
        {
            return this.SendCommand( "xmllist" );
        }

        public string SendCommand( string a_commandString )
        {
            //-- Each FHEM command must end with "\r\n"
            if( !a_commandString.EndsWith( "\r\n" ) )
            {
                a_commandString = a_commandString + "\r\n";
            }

            using( var networkStream = m_tcpClient.GetStream() )
            {
                var writeBuffer = Encoding.ASCII.GetBytes( a_commandString );

                networkStream.Write( writeBuffer, 0, writeBuffer.Length );
                networkStream.Flush();

                var readingDateTime = DateTime.Now;

                var readBuffer = new byte[this.ReadBufferSize];

                StringBuilder stringBuilder = null;

                do
                {
                    if( networkStream.DataAvailable )
                    {
                        var readBytesCount = networkStream.Read( readBuffer, 0, this.ReadBufferSize );

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
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------