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

        public HttpRequest(string headData)
        {
            Method = HttpRequestParseHelper.GetMethod(headData);
            Path = HttpRequestParseHelper.GetPath(headData);
            _headers = HttpRequestParseHelper.GetHeaders(headData);

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