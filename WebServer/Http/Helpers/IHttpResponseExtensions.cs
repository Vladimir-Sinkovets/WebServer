using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Enums;
using WebServer.Http.Interfaces;

namespace WebServer.Http.Helpers
{
    internal static class IHttpResponseExtensions
    {
        public static byte[] BuildResponseMessage(this IHttpResponse response)
        {
            StringBuilder responseBuilder = new StringBuilder($"HTTP/{response.HttpVersion} ");

            responseBuilder.Append($"{response.StatusCode} \n");

            return CreateResponse(response, responseBuilder);
        }

        private static byte[] CreateResponse(IHttpResponse response, StringBuilder responseBuilder)
        {
            byte[] header = CreateHeader(response, responseBuilder);

            if (response.Body != null)
            {
                return header.Concat(response.Body).ToArray();
            }
            else
            {
                return header;
            }
        }

        private static byte[] CreateHeader(IHttpResponse response, StringBuilder responseBuilder)
        {
            if (response.Cookie != null && response.Cookie.Count() != 0)
            {
                responseBuilder.Append($"Set-Cookie: {response.Cookie.ConvertToString()}\n");
            }

            if (response.Body != null)
            {
                responseBuilder.Append($"Content-Length: {response.Body.Length}\n");
            }

            foreach (var header in response.Headers)
            {
                responseBuilder.Append($"{header.Key}: {header.Value}\n");
            }

            responseBuilder.Append($"\n");

            return Encoding.ASCII.GetBytes(responseBuilder.ToString());
        }
    }
}
