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
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Basics
{
    public class FhemObjectRepository : /* ObservableCollection<string>, */ IEnumerable, INotifyCollectionChanged
    {
        //---------------------------------------------------------------------
        #region Fields

        private ObservableCollection<FhemObject>  m_fhemObjectCollection = new ObservableCollection<FhemObject>();

        //-- Fields
        #endregion
        //---------------------------------------------------------------------
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FhemObjectRepository class.
        /// </summary>
        /// <remarks>
        /// The constructor is private to force the usage of the static 
        /// 'Create' method (long lasting operations do not belong into a 
        /// constructor ;).
        /// </remarks>
        private FhemObjectRepository()
        {
            //-- Register to events
            m_fhemObjectCollection.CollectionChanged += fhemObjectCollection_CollectionChanged;
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region Event Handlers

        private void fhemObjectCollection_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            this.CollectionChanged?.Invoke( this, e );
        }

        //-- Event Handlers
        #endregion
        //---------------------------------------------------------------------
        #region IEnumerable

        public IEnumerator GetEnumerator()
        {
            return m_fhemObjectCollection.GetEnumerator();
        }

        //-- IEnumerable
        #endregion
        //---------------------------------------------------------------------
        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        //-- INotifyCollectionChanged
        #endregion
        //---------------------------------------------------------------------
        #region Methods

        /// <summary>
        /// Creates a FhemObjectRepository instance by parsing a corresponding
        /// JSON object.
        /// </summary>
        /// <param name="a_jsonObject"></param>
        /// <exception cref="ArgumentNullException">
        /// The JSON object may not be null.
        /// </exception>
        /// <returns>
        /// The created FhemObjectRepository instance.
        /// </returns>
        public static FhemObjectRepository Create( JObject a_jsonObject )
        {
            if( a_jsonObject == null )
            {
                throw new ArgumentNullException( "The JSON object may not be null!" );
            }

            var me = new FhemObjectRepository();

            //-- Determine the 3 main json tokens
            var argJsonToken = a_jsonObject.First;
            var resultsJsonToken = argJsonToken.Next;
            var totalResultsJsonToken = resultsJsonToken.Next;

            //-- Determine the first json token that represents a fhem object
            var fhemObjectAsJsonObject = (JObject) resultsJsonToken.First.First;

            while( fhemObjectAsJsonObject != null )
            {
                var fhemObject = FhemObject.FromJObject( fhemObjectAsJsonObject );

                me.m_fhemObjectCollection.Add( fhemObject );

                fhemObjectAsJsonObject = (JObject) fhemObjectAsJsonObject.Next;
            }

            return me;
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------