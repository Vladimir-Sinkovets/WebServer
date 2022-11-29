using System;
using System.Linq;
using System.Text;
using WebServer.Models;
using WebServer.Models.CookieCollections;
using WebServer.Services.HttpContextFactories.Helpers;

namespace WebServer.Services.HttpContextFactories
{
    public class HttpContextFactory : IHttpContextFactory
    {
        public HttpContext CreateInstance(byte[] data, IServiceProvider provider)
        {
            int headDataLength = GetEndOfHeaderPosition(data);

            string headData = Encoding.UTF8.GetString(data, 0, headDataLength);

            byte[] body = data
                .Skip(headDataLength)
            .ToArray();

            HttpRequest request = ParseRequest(headData, body);
            HttpResponse response = new();

            return new HttpContext(request, response, provider);
        }

        private static HttpRequest ParseRequest(string headData, byte[] body)
        {
            var request = new HttpRequest()
            {
                Path = HttpRequestParseHelper.GetPath(headData),
                Query = HttpRequestParseHelper.GetQueryParameters(headData),
                Method = HttpRequestParseHelper.GetMethod(headData),
                Headers = HttpRequestParseHelper.GetHeaders(headData),
                QueryString = HttpRequestParseHelper.GetQueryString(headData),

                Body = body
            };

            if (request.Headers.ContainsKey("cookie"))
            {
                var cookiePairs = HttpRequestParseHelper.GetCookieDictionary(request.Headers["cookie"]);
                request.Cookie = new RequestCookieCollection(cookiePairs);
            }
            else
            {
                request.Cookie = new RequestCookieCollection();
            }

            return request;
        }

        private static int GetEndOfHeaderPosition(byte[] bytes)
        {
            int i = 0;

            while (!((char)bytes[i + 1] == '\r' &&
                   (char)bytes[i + 2] == '\n' &&
                   (char)bytes[i + 3] == '\r' &&
                   (char)bytes[i + 4] == '\n'))
            {
                i++;
            }

            return i + 1;
        }
    }
}
