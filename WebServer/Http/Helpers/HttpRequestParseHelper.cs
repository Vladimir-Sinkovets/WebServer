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
        public static string GetPath(string httpData)
        {
            string url = GetUrl(httpData);

            int pos = url.IndexOf('?');

            if (pos == -1)
                return url;

            return url.Substring(0, pos);
        }

        public static string GetQueryString(string httpData)
        {
            string url = GetUrl(httpData);

            int pos = url.IndexOf('?');

            if (pos == -1)
                return string.Empty;

            return url.Substring(pos).Replace("?", "");
        }

        public static HttpMethod GetMethod(string httpData)
        {
            Regex methodRegex = new Regex(@"^\w+", RegexOptions.IgnoreCase);
            string methodName = methodRegex.Match(httpData).Value
                .ToLower();

            return methodName switch
            {
                "get" => HttpMethod.GET,
                "put" => HttpMethod.PUT,
                "post" => HttpMethod.POST,
                "head" => HttpMethod.HEAD,
                "delete" => HttpMethod.DELETE,
                "connect" => HttpMethod.CONNECT,
                "options" => HttpMethod.OPTIONS,
                "trace" => HttpMethod.TRACE,
                _ => throw new ArgumentException("Wrong http method name"),
            };
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

        public static IDictionary<string, string> GetQueryParameters(string httpData)
        {
            IDictionary<string, string> queryParameters = new Dictionary<string, string>();

            string queryString = GetQueryString(httpData);

            if (queryString == string.Empty)
                return queryParameters;

            string[] parameters = queryString.Split('&');

            foreach (string parameter in parameters)
            {
                string[] pair = parameter.Split('=');

                queryParameters.Add(pair[0], pair[1]);
            }

            return queryParameters;
        }

        public static IDictionary<string, string> GetCookieDictionary(string cookieData)
        {
            IDictionary<string, string> pairs = new Dictionary<string, string>();

            cookieData = cookieData.Replace(" ", "");

            string[] pairsString = cookieData.Split(';');

            foreach (var pairString in pairsString)
            {
                string[] pair = pairString.Split('=');

                pairs.Add(pair[0], pair[1]);
            }

            return pairs;
        }

        private static string GetUrl(string httpData)
        {
            Regex pathRegex = new Regex(@"(?<=^\w+\s).+(?= )", RegexOptions.IgnoreCase);

            string url = pathRegex.Match(httpData).Value
                .ToLower();
            return url;
        }
    }
}