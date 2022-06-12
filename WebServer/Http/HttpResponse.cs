using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Enums;
using WebServer.Http.Interfaces;

namespace WebServer.Http
{
    public class HttpResponse : IHttpResponse
    {
        public byte[] Body { get; set; }
        public string Connection
        {
            get => Headers["Connection"];
            set => SetHeaderValue("Connection", value);
        }
        public string ContentType
        {
            get => Headers["Content-Type"];
            set => SetHeaderValue("Content-Type", value);
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
            ContentType = "text/html";
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