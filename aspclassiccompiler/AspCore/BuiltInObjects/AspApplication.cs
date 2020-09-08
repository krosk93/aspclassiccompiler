using AspCore.BuiltInObjects.Abstractions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AspCore.BuiltInObjects
{
    /// <summary>
    /// The application object that is accessible from ASP code
    /// </summary>
    public class AspApplication : IAspApplication
	{
        private readonly AspApplicationContents contents = new AspApplicationContents();


        #region IApplicationObject Members

        public IVariantDictionary Contents
        {
            get { return contents; }
        }

        public IVariantDictionary StaticObjects
        {
            get { throw new NotImplementedException(); }
        }

        public bool Locked
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Lock()
        {
            throw new NotImplementedException();
        }

        public void UnLock()
        {
            throw new NotImplementedException();
        }

        public object this[string key]
        {
            get { return contents[key]; }
            set { contents[key] = value; }
        }

        public void let_Value(string bstrValue, object pvar)
        {
            this[bstrValue] = pvar;
        }

        #endregion
    }

    public class AspApplicationContents : IVariantDictionary
    {
        private readonly ConcurrentDictionary<string, byte[]> dic = new ConcurrentDictionary<string, byte[]>();

        public dynamic this[object VarKey] { get => get_Key(VarKey); set => let_Item(VarKey, value); }

        public int Count => dic.Count;

        public IEnumerator GetEnumerator()
        {
            return dic.Keys.GetEnumerator();
        }

        public dynamic get_Key(object VarKey)
        {
            if (dic.TryGetValue(VarKey.ToString(), out var bytes))
            {
                using (MemoryStream stream = new MemoryStream(bytes))
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
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, pvar);
                bytes = stream.ToArray();

            }
            _ = dic.AddOrUpdate(VarKey.ToString(), bytes, (x, y) => bytes);
        }

        public void Remove(object VarKey)
        {
            if (!dic.TryRemove(VarKey.ToString(), out _))
                throw new ArgumentOutOfRangeException(nameof(VarKey));
        }

        public void RemoveAll()
        {
            dic.Clear();
        }
    }
}
