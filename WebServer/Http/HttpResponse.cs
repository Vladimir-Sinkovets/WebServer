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
        public string HttpVersion { get; set; }
        public StatusCode StatusCode { get; set; } = StatusCode.OK;
        public string Content { get; set; }
        public string Connection
        {
            get => _headers["Connection"];
            set => SetHeaderValue("Connection", value);
        }
        public string ContentType
        {
            get => _headers["Content-Type"];
            set => SetHeaderValue("Content-Type", value);
        }
        public IResponseCookie Cookie { get; }

        private IDictionary<string, string> _headers = new Dictionary<string, string>();

        public HttpResponse()
        {
            Cookie = new ResponseCookie();
            HttpVersion = "1.1";
            StatusCode = StatusCode.OK;
            ContentType = "text/html";
            Connection = "Closed";
        }

        public override string ToString()
        {
            StringBuilder response = new StringBuilder($"HTTP/{HttpVersion} {StatusCode}\n");

            response.Append($"Set-Cookie: {Cookie.ToString()}\n");
            response.Append($"Content-Length: {Content.Length}\n");

            foreach (var header in _headers)
            {
                response.Append($"{header.Key}: {header.Value}\n");
            }

            response.Append($"\n{Content}");

            return response.ToString();
        }

        private void SetHeaderValue(string headerName, string headerValue)
        {
            if (!_headers.ContainsKey(headerName))
            {
                _headers.Add(headerName, headerValue);
            }
            else
            {
                _headers[headerName] = headerValue;
            }
        }
    }
}