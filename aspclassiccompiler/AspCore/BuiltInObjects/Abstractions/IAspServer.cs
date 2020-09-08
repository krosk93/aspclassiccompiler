namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IAspServer
    {
        dynamic CreateObject(string bstrProgID);
        string HTMLEncode(string bstrIn);
        string MapPath(string bstrLogicalPath);
        string URLEncode(string bstrIn);
        string URLPathEncode(string bstrIn);
        void Execute(string bstrLogicalPath);
        void Transfer(string bstrLogicalPath);
        IAspError GetLastError();
        int ScriptTimeout { get; set; }
    }
}
