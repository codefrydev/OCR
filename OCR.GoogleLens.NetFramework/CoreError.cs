using System;
using System.Net.Http.Headers;

namespace OCR.GoogleLens.NetFramework
{
    public class CoreError : Exception
    {
        public int Code { get; private set; }
        public HttpResponseHeaders Headers { get; private set; }
        public string Body { get; private set; }
        public CoreError(string message, int code, HttpResponseHeaders headers, string body)
            : base(message)
        {
            Code = code;
            Headers = headers;
            Body = body;
        }
    }
}
