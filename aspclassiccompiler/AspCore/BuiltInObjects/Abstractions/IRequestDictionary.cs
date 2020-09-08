using System.Collections;

namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IRequestDictionary : IEnumerable
    {
        new IEnumerator GetEnumerator();
        dynamic get_Key(object VarKey);
        dynamic this[object Var] { get; }
        int Count { get; }
    }
}
