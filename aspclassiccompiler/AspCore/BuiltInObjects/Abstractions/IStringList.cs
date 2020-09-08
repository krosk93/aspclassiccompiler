using System.Collections;

namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IStringList : IEnumerable
    {
        new IEnumerator GetEnumerator();
        dynamic this[object i] { get; }
        int Count { get; }
    }
}
