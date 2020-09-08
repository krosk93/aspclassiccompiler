using AspCore.BuiltInObjects.Abstractions;
using Microsoft.AspNetCore.Http;
using System;

namespace AspCore.BuiltInObjects
{
    public class AspResponseCookieCollection : IRequestDictionary
    {
        private IResponseCookies _cookiecollection;

        #region constructor
        public AspResponseCookieCollection(IResponseCookies cookieCollection)
        {
            _cookiecollection = cookieCollection;
        }
        #endregion

        #region IRequestDictionary Members

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public object get_Key(object VarKey)
        {
            throw new NotImplementedException();
        }

        public object this[object key]
        {
            get 
            {
                throw new NotImplementedException();
            }

            set
            {
                if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
                {
                    _cookiecollection.Delete(key.ToString());
                }
                else
                {
                    _cookiecollection.Append(key.ToString(), value.ToString());
                }
            }
        }

        #endregion
    }
}
