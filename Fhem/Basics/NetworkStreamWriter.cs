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
using System.Net.Sockets;
using System.Text;
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Basics
{
    public class NetworkStreamWriter
    {
        //---------------------------------------------------------------------
        #region Fields

        private NetworkStream  m_networkStream;

        //-- Fields
        #endregion
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the NetworkStreamWriter class.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// The network stream may not be null.
        /// </exception>
        public NetworkStreamWriter( NetworkStream a_networkStream )
        {
            if( a_networkStream == null )
            {
                throw new ArgumentException( "The network stream may not be null!" );
            }

            m_networkStream = a_networkStream;
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Writes a string to the underlying network stream.
        /// </summary>
        /// <param name="a_string">
        /// The string that should be written to the network stream.
        /// </param>
        public void Write( string a_string )
        {
            //-- Convert the native string into a byte array
            var writeBuffer = Encoding.ASCII.GetBytes( a_string );

            //-- Send the byte array
            m_networkStream.Write( writeBuffer, 0, writeBuffer.Length );
            m_networkStream.Flush();
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------