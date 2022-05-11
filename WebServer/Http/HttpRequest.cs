using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebServer.Http;
using WebServer.Http.Cookie;

namespace WebServer
{
    public class HttpRequest
    {
        public HttpMethod Method { get; }
        public string Url { get; }
        public IRequestCookieCollection Cookie { get; }
        //public string Cookie { get => _headers["Cookie"]; }

        private IDictionary<string, string> _headers = new Dictionary<string, string>();

        public HttpRequest(string httpData)
        {
            Method = HttpRequestParser.GetMethod(httpData);
            Url = HttpRequestParser.GetUrl(httpData);
            _headers = HttpRequestParser.GetHeaders(httpData);

            Cookie = new RequestCookieCollection(httpData);
        }
        public string GetHeaderValue(string headerName)
        {
            return _headers[headerName];
        }
    }
}