using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebServer.Enums;

namespace WebServer.Http.Helpers
{
    internal static class HttpRequestParseHelper
    {
        public static HttpMethod GetMethod(string httpData)
        {
            Regex methodRegex = new Regex(@"^\w+", RegexOptions.IgnoreCase);
            string methodName = methodRegex.Match(httpData).Value
                .ToLower();

            switch (methodName)
            {
                case "get":
                    return HttpMethod.GET;
                case "put":
                    return HttpMethod.PUT;
                case "post":
                    return HttpMethod.POST;
                case "head":
                    return HttpMethod.HEAD;
                case "delete":
                    return HttpMethod.DELETE;
                case "connect":
                    return HttpMethod.CONNECT;
                case "options":
                    return HttpMethod.OPTIONS;
                case "trace":
                    return HttpMethod.TRACE;
                default:
                    throw new ArgumentException("Wrong http method name");
            }
        }

        public static string GetPath(string httpData)
        {
            Regex pathRegex = new Regex(@"(?<=^\w+\s).+(?= )", RegexOptions.IgnoreCase);
            string path = pathRegex.Match(httpData).Value
                .ToLower();

            return path;
        }

        public static IDictionary<string, string> GetHeaders(string httpData)
        {
            Regex headerRegex = new Regex(@".+:.+");

            MatchCollection headerMatches = headerRegex.Matches(httpData);

            IDictionary<string, string> headers = new Dictionary<string, string>();

            foreach (Match match in headerMatches)
            {
                Regex headerNameRegex = new Regex(@"^.+(?=:)");
                string headerName = headerNameRegex.Match(match.Value).Value;

                Regex headerValueRegex = new Regex(@"(?<=: *)[^\s].+");
                string headerValue = headerValueRegex.Match(match.Value).Value;

                headers.Add(headerName, headerValue);
            }

            return headers;
        }


    }
}
