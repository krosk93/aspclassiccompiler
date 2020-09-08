using AspCore.BuiltInObjects.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Collections;
using System.Collections.Generic;

namespace AspCore.BuiltInObjects
{
    public class AspServerVariables : IRequestDictionary
    {
        private readonly HttpContext context;
        private readonly Dictionary<string, string> contents;

        public AspServerVariables(HttpContext context)
        {
            this.context = context;
            contents = new Dictionary<string, string>()
            {
                { "LOCAL_ADDR", context.Connection.LocalIpAddress.ToString() },
                { "REMOTE_ADDR", context.Connection.RemoteIpAddress.ToString() }
            };
        }
        public dynamic this[object Var] => get_Key(Var);

        public int Count => contents.Count;

        public IEnumerator GetEnumerator()
        {
            return contents.Keys.GetEnumerator();
        }

        public dynamic get_Key(object VarKey)
        {
            return contents[VarKey.ToString()];
        }
    }
}
