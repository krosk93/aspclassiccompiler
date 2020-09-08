using AspCore.BuiltInObjects.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AspCore.BuiltInObjects
{
    /// <summary>
    /// The session object that is accessible from ASP code
    /// </summary>
    public class AspSession : IAspSession
    {
        private readonly HttpContext context;

        public AspSession(IHttpContextAccessor contextAccessor)
        {
            context = contextAccessor.HttpContext;
            Contents = new AspSessionContents(context.Session);
        }

        public object this[string key]
        {
            get { return Contents[key]; }
            set { Contents[key] = value; }
        }

        public object StaticObjects
        {
            get { throw new NotImplementedException(); }
        }

        public IVariantDictionary Contents { get; }

        public void Abandon()
        {
            context.Session.Clear();
        }

        public int CodePage
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int LCID
        {
            get { return CultureInfo.CurrentCulture.LCID; }
            set { throw new NotImplementedException(); }
        }

        public string SessionID
        {
            get { return context.Session.Id; }
        }

        public int TimeOut
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }

    public class AspSessionContents : IVariantDictionary
    {
		private readonly ISession session;
		public AspSessionContents(ISession session)
        {
			this.session = session;
        }

        public dynamic this[object VarKey] { get => get_Key(VarKey); set => let_Item(VarKey, value); }

        public int Count => session.Keys.Count();

        public IEnumerator GetEnumerator()
        {
            return session.Keys.GetEnumerator();
        }

        public dynamic get_Key(object VarKey)
        {
			if(session.TryGetValue(VarKey.ToString(), out var bytes))
			{
				using(MemoryStream stream = new MemoryStream(bytes))
				{
					IFormatter formatter = new BinaryFormatter();
					return formatter.Deserialize(stream);
				}
			}
			return null;
        }

        public void let_Item(object VarKey, object pvar)
        {
			byte[] bytes;
			IFormatter formatter = new BinaryFormatter();
			using(MemoryStream stream = new MemoryStream())
            {
				formatter.Serialize(stream, pvar);
				bytes = stream.ToArray();
				
            }
			session.Set(VarKey.ToString(), bytes);
        }

        public void Remove(object VarKey)
        {
			session.Remove(VarKey.ToString());
        }

        public void RemoveAll()
        {
			session.Clear();
        }
    }
}
