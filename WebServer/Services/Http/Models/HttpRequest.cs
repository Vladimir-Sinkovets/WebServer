using System.Collections.Generic;
using WebServer.Services.Http.CookieCollections;
using WebServer.Services.Http.Enums;

namespace WebServer.Services.Http.Models
{
    public class HttpRequest
    {
        public string Path { get; set; }
        public byte[] Body { get; set; }
        public string ContentType
        {
            get => Headers["content-type"];
            set => SetHeaderValue("content-type", value);
        }
        public string QueryString { get; set; }
        public HttpMethod Method { get; set; }
        public IDictionary<string, string> Query { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IRequestCookieCollection Cookie { get; set; }


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