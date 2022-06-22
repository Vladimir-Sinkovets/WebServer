using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebServer.Enums;
using WebServer.Http.Helpers;
using WebServer.Http.Interfaces;

namespace WebServer.Http.Models
{
    public class HttpRequest : IHttpRequest
    {
        public string Path { get; }
        public byte[] Body { get; }
        public string ContentType
        {
            get => Headers["content-type"];
            set => SetHeaderValue("content-type", value);
        }
        public string QueryString { get; }
        public HttpMethod Method { get; }
        public IDictionary<string, string> Query { get; }
        public IDictionary<string, string> Headers { get; }
        public IRequestCookieCollection Cookie { get; }

        public HttpRequest(string headData, byte[] body)
        {
            Path = HttpRequestParseHelper.GetPath(headData);
            Query = HttpRequestParseHelper.GetQueryParameters(headData);
            Method = HttpRequestParseHelper.GetMethod(headData);
            Headers = HttpRequestParseHelper.GetHeaders(headData);
            QueryString = HttpRequestParseHelper.GetQueryString(headData);

            Body = body;

            if (Headers.ContainsKey("cookie"))
            {
                Cookie = new RequestCookieCollection(Headers["cookie"]);
            }
            else
            {
                Cookie = new RequestCookieCollection();
            }
        }

        private void SetHeaderValue(string headerName, string headerValue)
        {
            if (!Headers.ContainsKey(headerName))
            {
                Headers.Add(headerName, headerValue);
            }
            else
            {
                Headers[headerName] = headerValue;
            }
        }
    }
}