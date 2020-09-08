using System.Collections.Generic;
using System.Reflection;

namespace AspCore
{
    public class AspHostConfiguration
    {
        public IList<Assembly> Assemblies { get; set; }
        public bool Trace { get; set; }
    }
}
