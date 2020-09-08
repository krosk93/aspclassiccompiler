namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IAspSession
    {
        object this[string key] { get; set; }

        int CodePage { get; set; }
        IVariantDictionary Contents { get; }
        int LCID { get; set; }
        string SessionID { get; }
        object StaticObjects { get; }
        int TimeOut { get; set; }

        void Abandon();
    }
}