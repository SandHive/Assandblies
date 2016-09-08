using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Basics
{
    public class NetworkStreamReader : IDisposable
    {
        //---------------------------------------------------------------------
        #region Fields

        private BinaryReader  m_binaryReader;

        private NetworkStream  m_networkStream;

        //-- Fields
        #endregion
        //---------------------------------------------------------------------
        #region Properties

        /// <summary>
        /// Gets or sets the time span that is waited for a network stream 
        /// response.
        /// </summary>
        public TimeSpan WaitingTimeSpanForResponse { get; set; } = TimeSpan.FromMilliseconds( 250 );

        //-- Properties
        #endregion
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the NetworkStreamReader class.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The network stream may not be null.
        /// </exception>
        public NetworkStreamReader( NetworkStream a_networkStream )
        {
            if( a_networkStream == null )
            {
                throw new ArgumentException( "The network stream may not be null!" );
            }

            m_networkStream = a_networkStream;
            m_binaryReader = new BinaryReader( m_networkStream, Encoding.ASCII, true );
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region IDisposable Members

        public void Dispose()
        {
            m_binaryReader.Dispose();
        }

        //-- IDisposable Members
        #endregion
        //---------------------------------------------------------------------
        #region Methods

        public string ReadString()
        {
            var referenceDateTime = DateTime.Now;

            var pastTimSpan = new TimeSpan();

            var resultBytes = new List<byte>();

            do
            {
                if( m_networkStream.DataAvailable )
                {
                    //-- Reading single bytes is more reliable than reading strings or several bytes at once
                    resultBytes.Add( m_binaryReader.ReadByte() );

                    //-- Update the reference date time
                    referenceDateTime = DateTime.Now;
                }

                //-- Update the past time span till the last successful reading
                pastTimSpan = (TimeSpan) ( DateTime.Now - referenceDateTime );
            }
            while( pastTimSpan < this.WaitingTimeSpanForResponse );

            return Encoding.ASCII.GetString( resultBytes.ToArray() );
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------