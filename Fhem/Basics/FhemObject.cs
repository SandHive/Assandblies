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
//-----------------------------------------------------------------------------
namespace Sand.Fhem.Basics
{
    public class FhemObject
    {
        //---------------------------------------------------------------------
        #region Properties

        public object Attributes { get; private set; }

        public object Internals { get; private set; }

        public string Name { get; private set; }

        public string PossibleAttributes { get; private set; }

        public string PossibleSets { get; private set; }

        public object Readings { get; private set; }

        //-- Properties
        #endregion
        //---------------------------------------------------------------------
        #region Constructors


        private FhemObject()
        {
           
        }

        //-- Constructors
        #endregion
        //---------------------------------------------------------------------
        #region Methods


        public static FhemObject FromJObject( JObject a_jsonObject )
        {
            //-- Validate argument
            if( a_jsonObject == null )
            {
                throw new ArgumentNullException( "The json object may not be null!" );
            }

            if( a_jsonObject.Count != 6 )
            {
                throw new ArgumentOutOfRangeException( "The json object must have 6 children!" );
            }
            
            //-- Create the new fhem object
            var me = new FhemObject();
            
            //-- Analyze the first json property
            foreach( var jsonProperty in a_jsonObject.Children<JProperty>() )
            {
                switch( jsonProperty.Name )
                {
                    case "Attributes": me.Attributes = jsonProperty.Value; break;

                    case "Internals": me.Internals = jsonProperty.Value; break;

                    case "Name": me.Name = (string) jsonProperty.Value; break;

                    case "PossibleAttrs": me.PossibleAttributes = (string) jsonProperty.Value; break;

                    case "PossibleSets": me.PossibleSets = (string) jsonProperty.Value; break;

                    case "Readings": me.Readings = jsonProperty.Value; break;

                    default: break;
                }
            }
            
            return me;
        }

        //-- Methods
        #endregion
        //---------------------------------------------------------------------
    }
}
//-----------------------------------------------------------------------------