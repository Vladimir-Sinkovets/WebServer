using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class HttpResponse
    {
        private IDictionary<string, string> _headers = new Dictionary<string, string>();
        public string HttpVersion { get; set; } = "1.1";
        public string Content { get; set; }
        public string ContentType { get; set; } = "text/html";
        public string StatusCode { get; set; } = "200";

        public void AddHeader(string header, string value)
        {
            _headers.Add(header, value);
        }

        public override string ToString()
        {
            StringBuilder response = new StringBuilder($"HTTP/{HttpVersion} {StatusCode}\n");

            response.Append(
                $"Content-Type: {ContentType}\n" +
                $"Content-Length: {Content.Length}\n");

            foreach (var header in _headers)
            {
                response.Append($"{header.Key}: {header.Value}\n");
            }
            response.Append($"\n{Content}");
            return response.ToString();
        }
    }
}