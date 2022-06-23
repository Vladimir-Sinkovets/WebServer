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
            StringBuilder responseHeader = new StringBuilder($"HTTP/{response.HttpVersion} ");

            switch (response.StatusCode)
            {
                case StatusCode.OK:
                    responseHeader.Append($"200 {response.StatusCode} \n");
                    break;
                default:
                    throw new NotImplementedException($"There is no implementation for {response.StatusCode} case");
            }

            if (response.Cookie != null && response.Cookie.Count() != 0)
            {
                responseHeader.Append($"Set-Cookie: {response.Cookie.ConvertToString()}\n");
            }

            if (response.Body != null)
            {
                responseHeader.Append($"Content-Length: {response.Body.Length}\n");
            }

            foreach (var header in response.Headers)
            {
                responseHeader.Append($"{header.Key}: {header.Value}\n");
            }

            responseHeader.Append($"\n");

            byte[] headerOfMessage = Encoding.ASCII.GetBytes(responseHeader.ToString());

            if (response.Body != null)
            {
                return headerOfMessage.Concat(response.Body).ToArray();
            }
            else
            {
                return headerOfMessage;
            }
        }
    }
}
