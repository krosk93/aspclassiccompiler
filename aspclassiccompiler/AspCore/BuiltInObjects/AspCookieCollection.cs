using AspCore.BuiltInObjects.Abstractions;
using Microsoft.AspNetCore.Http;
using System;

namespace Dlrsoft.Asp.BuiltInObjects
{
    public class AspCookieCollection : IRequestDictionary
    {
        private IResponseCookies _cookiecollection;
        private bool _request;

        #region constructor
        public AspCookieCollection(IRequestCookieCollection cookieCollection, bool request)
        {
            _cookiecollection = cookieCollection;
            _request = request;
        }
        #endregion

        #region IRequestDictionary Members

        public int Count
        {
            get { return _cookiecollection.Count; }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return _cookiecollection.GetEnumerator();
        }

        public object get_Key(object VarKey)
        {
            throw new NotImplementedException();
        }

        public object this[object key]
        {
            get 
            {
                string cookie = null;
                if (key is int keyCode)    
                    cookie = _cookiecollection[keyCode.ToString()]; 
                else
                    cookie = _cookiecollection[(string)key];

                if (cookie == null)
                    return "";

                if (_request)
                    return new AspReadCookie(cookie);
                else
                    return new AspWriteCookie(cookie);
            }

            set
            {
                if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
                {
                    _cookiecollection.Remove(key.ToString());
                }
            }
        }

        #endregion
    }
}
