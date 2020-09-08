using System;

namespace AspCore.BuiltInObjects.Abstractions
{
    public interface IAspResponse
    {
        bool Buffer { get; set; }
        string CacheControl { get; set; }
        string CharSet { get; set; }
        int CodePage { get; set; }
        string ContentType { get; set; }
        AspResponseCookieCollection Cookies { get; }
        int Expires { get; set; }
        DateTime ExpiresAbsolute { get; set; }
        int LCID { get; set; }
        string Status { get; set; }

        void Add(string bstrHeaderValue, string bstrHeaderName);
        void AddHeader(string name, string value);
        void AppendToLog(string param);
        void BinaryWrite(byte[] varInput);
        void Clear();
        void End();
        void Flush();
        bool IsClientConnected();
        void Pics(string value);
        void Redirect(string url);
        void Write(object output);
        void WriteBlock(short iBlockNumber);
    }
}