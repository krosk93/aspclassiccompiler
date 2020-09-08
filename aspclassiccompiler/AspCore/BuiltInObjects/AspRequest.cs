using AspCore.BuiltInObjects.Abstractions;
using Microsoft.AspNetCore.Http;
using System;

namespace AspCore.BuiltInObjects
{
    public class AspRequest : IAspRequest
    {
        private readonly HttpContext context;

        #region constructor

		public AspRequest(IHttpContextAccessor contextAccessor)
		{
			context = contextAccessor.HttpContext;
		}
        #endregion

        #region IRequest Members

        public object BinaryRead(ref object pvarCountToRead)
        {
            throw new NotImplementedException();
        }

        public IRequestDictionary Body
        {
            get { throw new NotImplementedException(); }
        }

        public IRequestDictionary ClientCertificate
        {
            get { throw new NotImplementedException(); }
        }

        public IRequestDictionary Cookies
        {
            get
            {
                throw new NotImplementedException();
                //return new AspCookieCollection(context.Request.Cookies, true);
            }
        }

        public IRequestDictionary Form
        {
            get {
                throw new NotImplementedException();
                //return new AspNameValueCollection(context.Request.Form); 
            }
        }

        public IRequestDictionary QueryString
        {
            get {
                throw new NotImplementedException(); 
                //return new AspNameValueCollection(context.Request.Query);
            }
        }

        public IRequestDictionary ServerVariables
        {
            get
            {
                return new AspServerVariables(context);
            }
        }

        public int TotalBytes
        {
            get
            {
                throw new NotImplementedException(); 
                //return context.Request.TotalBytes;
            }
        }

        public object this[string key]
        {
            get
            {

                throw new NotImplementedException();
                //if (context.Request.Form[key] != null)
                //    return context.Request.Form[key];
                //else if (context.Request.QueryString[key] != null)
                //    return context.Request.QueryString[key];
                //else
                //    return "";
            }
        }

        #endregion
    }
}
