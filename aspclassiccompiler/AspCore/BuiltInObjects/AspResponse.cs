using AspCore.BuiltInObjects.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace AspCore.BuiltInObjects
{
    /// <summary>
    /// The response object that is accessible from ASP code
    /// Get not implement IResponse because the COM interface not supported by C#
    /// </summary>
    public class AspResponse : IAspResponse
    //: IResponse
    {
        private readonly StreamWriter sw;
        private readonly HttpContext context;
        private readonly ILogger<AspResponse> logger;
        private readonly Lazy<AspResponseCookieCollection> aspResponseCookieCollection;

        public AspResponse(IHttpContextAccessor httpContextAccessor, ILogger<AspResponse> logger)
        {
            context = httpContextAccessor.HttpContext;
            sw = new StreamWriter(context.Response.Body)
            {
                AutoFlush = false
            };
            this.logger = logger;
            this.aspResponseCookieCollection = new Lazy<AspResponseCookieCollection>(
                () => new AspResponseCookieCollection(context.Response.Cookies)
            );
        }

        #region IResponse Members

        public void Add(string bstrHeaderValue, string bstrHeaderName)
        {
            AddHeader(bstrHeaderName, bstrHeaderValue);
        }

        public void AddHeader(string name, string value)
        {
            context.Response.Headers.Add(name, value);
        }

        public void AppendToLog(string param)
        {
            logger.LogInformation(param);
        }

        public void BinaryWrite(byte[] varInput)
        {
            context.Response.Body.Write(varInput, 0, varInput.Length);
        }

        public void Clear()
        { context.Response.Clear(); }

        public void End()
        { context.Response.Body.Close(); }

        public void Flush()
        {
            sw.Flush();
            context.Response.Body.Flush();
        }

        public bool IsClientConnected()
        {
            return !context.RequestAborted.IsCancellationRequested;
        }

        public void Pics(string value)
        { throw new NotImplementedException(); }

        public void Redirect(string url)
        {
            context.Response.Redirect(url, true);
        }

        public void Write(object output)
        {
            if (output == null) return;

            string strOut = output as string;
            if (strOut == null)
            {
                Type t = output.GetType();
                while (t.IsCOMObject)
                {
                    output = t.InvokeMember(string.Empty,
                        BindingFlags.Default | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.GetProperty,
                        null,
                        output,
                        null);
                    t = output.GetType();
                }

                strOut = output.ToString();
            }
            sw.Write(strOut);
        }

        public void WriteBlock(short iBlockNumber)
        {
            throw new NotImplementedException();
        }

        public bool Buffer
        {
            get { return !sw.AutoFlush; }
            set { sw.AutoFlush = !value; }
        }

        public string CacheControl
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string CharSet
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int CodePage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ContentType
        {
            get { return context.Response.ContentType; }
            set { context.Response.ContentType = value; }
        }

        public AspResponseCookieCollection Cookies
        {
            get
            {
                return aspResponseCookieCollection.Value;
            }
        }

        public int Expires
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DateTime ExpiresAbsolute
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int LCID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Status
        {
            get { return context.Response.StatusCode.ToString(); }
            set { context.Response.StatusCode = int.Parse(value); }
        }

        #endregion
    }
}
