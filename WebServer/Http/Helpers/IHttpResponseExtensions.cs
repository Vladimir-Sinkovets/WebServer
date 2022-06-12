using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Http.Helpers
{
    public static class IHttpResponseExtensions
    {
        public static byte[] BuildResponseMessage(this IHttpResponse response)
        {
            StringBuilder responseHeader = new StringBuilder($"HTTP/{response.HttpVersion} {response.StatusCode}\n");

            responseHeader.Append($"Set-Cookie: {response.Cookie.ToString()}\n");
            responseHeader.Append($"Content-Length: {response.Body.Length}\n");

            foreach (var header in response.Headers)
            {
                responseHeader.Append($"{header.Key}: {header.Value}\n");
            }

            responseHeader.Append($"\n");

            byte[] headerOfMessage = Encoding.ASCII.GetBytes(responseHeader.ToString());

            return headerOfMessage.Concat(response.Body).ToArray();
        }
    }
}
