using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebServer.Enums;
using WebServer.Http.Helpers;
using WebServer.Http.Interfaces;

namespace WebServer.Http
{
    public class HttpRequest : IHttpRequest
    {
        public HttpMethod Method { get; }
        public string Path { get; }
        public IRequestCookieCollection Cookie { get; }

        private IDictionary<string, string> _headers = new Dictionary<string, string>();

        public HttpRequest(string httpData)
        {
            Method = HttpRequestParseHelper.GetMethod(httpData);
            Path = HttpRequestParseHelper.GetPath(httpData);
            _headers = HttpRequestParseHelper.GetHeaders(httpData);

            if (_headers.ContainsKey("Cookie"))
            {
                Cookie = new RequestCookieCollection(_headers["Cookie"]);
            }
            else
            {
                Cookie = new RequestCookieCollection();
            }
        }
        public string GetHeaderValue(string headerName)
        {
            return _headers[headerName];
        }
    }
}