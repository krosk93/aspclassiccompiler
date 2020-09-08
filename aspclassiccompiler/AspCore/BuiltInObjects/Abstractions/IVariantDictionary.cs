using System.Collections;

namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IVariantDictionary : IEnumerable
    {
        void let_Item(object VarKey, object pvar);
        dynamic get_Key(object VarKey);
        new IEnumerator GetEnumerator();
        void Remove(object VarKey);
        void RemoveAll();
        dynamic this[object VarKey] { get; set; }
        int Count { get; }
    }
}
