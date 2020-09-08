using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IAspApplication
    {
        void let_Value(string bstrValue, object pvar);
        void Lock();
        void UnLock();
        dynamic this[string bstrValue] { get; set; }
        IVariantDictionary StaticObjects { get; }
        IVariantDictionary Contents { get; }
    }
}
