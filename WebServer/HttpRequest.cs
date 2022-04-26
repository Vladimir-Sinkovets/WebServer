using System.Collections.Generic;

namespace WebServer
{
    internal class HttpRequest
    {
        public string Method { get; }
        public string Url { get; }
        private IDictionary<string, string> _headers = new Dictionary<string, string>;

        public HttpRequest(string httpData)
        {

        }
    }
}