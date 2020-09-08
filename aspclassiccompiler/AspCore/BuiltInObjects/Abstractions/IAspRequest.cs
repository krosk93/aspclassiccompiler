namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IAspRequest
    {
        dynamic BinaryRead(ref object pvarCountToRead);
        dynamic this[string bstrVar] { get; }
        IRequestDictionary QueryString { get; }
        IRequestDictionary Form { get; }
        IRequestDictionary Body { get; }
        IRequestDictionary ServerVariables { get; }
        IRequestDictionary ClientCertificate { get; }
        IRequestDictionary Cookies { get; }
        int TotalBytes { get; }
    }
}
