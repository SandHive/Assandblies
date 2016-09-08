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
            m_binaryReader?.Dispose();
        }

        //-- IDisposable Members
        #endregion
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Reads a string from the network stream.
        /// </summary>
        /// <returns>
        /// The string that was read from the network stream.
        /// </returns>
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