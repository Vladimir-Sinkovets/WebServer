using System.Linq;
using System.Text;
using WebServer.Models;

namespace WebServer.Services.HttpContextFactories.Helpers
{
    internal static class HttpResponseExtensions
    {
        public static byte[] BuildResponseMessage(this HttpResponse response)
        {
            StringBuilder responseBuilder = new();

            responseBuilder.Append($"HTTP/{response.HttpVersion} {response.StatusCode} \n");

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

        private static byte[] CreateHeader(HttpResponse response, StringBuilder responseBuilder)
        {
            if (response.Cookie != null && response.Cookie.Any())
            {
                responseBuilder.Append($"Set-Cookie: {response.Cookie.ConvertToString()}\n");
            }

            if (response.Body != null)
            {
                responseBuilder.Append($"Content-Length: {response.Body.Length}\n");
            }

            foreach (var header in response.Headers)
            {
                if (header.Value != null)
                    responseBuilder.Append($"{header.Key}: {header.Value}\n");
            }

            responseBuilder.Append('\n');

            return Encoding.ASCII.GetBytes(responseBuilder.ToString());
        }
    }
}