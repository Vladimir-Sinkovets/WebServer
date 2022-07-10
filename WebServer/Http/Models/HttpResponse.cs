using System.Collections.Generic;
using WebServer.Enums;
using WebServer.Http.Interfaces;

namespace WebServer.Http.Models
{
    public class HttpResponse : IHttpResponse
    {
        public byte[] Body { get; set; }
        public string Connection
        {
            get => Headers["connection"];
            set => SetHeaderValue("connection", value);
        }
        public string ContentType
        {
            get => Headers["content-type"];
            set => SetHeaderValue("content-type", value);
        }
        public string HttpVersion { get; set; }
        public StatusCode StatusCode { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IResponseCookieCollection Cookie { get; }

        public HttpResponse()
        {
            Cookie = new ResponseCookieCollection();
            Headers = new Dictionary<string, string>();
            HttpVersion = "1.1";
            StatusCode = StatusCode.OK;
            Connection = "Closed";
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