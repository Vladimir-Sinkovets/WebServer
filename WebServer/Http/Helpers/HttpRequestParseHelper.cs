using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebServer.Enums;
using WebServer.Http.Exceptions;

namespace WebServer.Http.Helpers
{
    internal static class HttpRequestParseHelper
    {
        public static string GetPath(string httpData)
        {
            if (httpData == null)
                throw new ArgumentNullException($"Parameter {nameof(httpData)} must not be null");
            if (httpData.Length == 0)
                throw new HttpParseException($"{nameof(httpData)} must not be empty");

            string url = GetUrl(httpData);

            int pos = url.IndexOf('?');

            if (pos == 0)
                throw new HttpParseException($"Path cannot have length 0 ({nameof(httpData)})");

            if (pos == -1)
                return url;

            return url.Substring(0, pos);
        }

        public static string GetQueryString(string httpData)
        {
            if (httpData == null)
                throw new ArgumentNullException($"Parameter {nameof(httpData)} must not be null");
            if (httpData.Length == 0)
                throw new HttpParseException($"{nameof(httpData)} must not be empty");

            string url = GetUrl(httpData);

            int pos = url.IndexOf('?');

            if (pos == -1)
                return string.Empty;

            return url.Substring(pos).Replace("?", "");
        }

        public static HttpMethod GetMethod(string httpData)
        {
            if (httpData == null)
                throw new ArgumentNullException($"Parameter {nameof(httpData)} must not be null");
            if (httpData.Length == 0)
                throw new HttpParseException($"{nameof(httpData)} must not be empty");

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
                _ => throw new HttpParseException("Wrong http method name"),
            };
        }

        public static IDictionary<string, string> GetHeaders(string httpData)
        {
            if (httpData == null)
                throw new ArgumentNullException($"Parameter {nameof(httpData)} must not be null");
            if (httpData.Length == 0)
                throw new HttpParseException($"{nameof(httpData)} must not be empty");

            Regex headerRegex = new Regex(@".+:.+");

            MatchCollection headerMatches = headerRegex.Matches(httpData);

            IDictionary<string, string> headers = new Dictionary<string, string>();

            foreach (Match match in headerMatches)
            {
                Regex headerNameRegex = new Regex(@"^.+(?=:)");
                string headerName = headerNameRegex.Match(match.Value).Value.ToLower();

                Regex headerValueRegex = new Regex(@"(?<=: *)[^\s].+");
                string headerValue = headerValueRegex.Match(match.Value).Value;

                if (headerName == string.Empty || headerValue == string.Empty)
                    throw new HttpParseException("Wrong headers' structure");

                headers.Add(headerName, headerValue);
            }

            return headers;
        }

        public static IDictionary<string, string> GetQueryParameters(string httpData)
        {
            if (httpData == null)
                throw new ArgumentNullException($"Parameter {nameof(httpData)} must not be null");
            if (httpData.Length == 0)
                throw new HttpParseException($"{nameof(httpData)} must not be empty");

            IDictionary<string, string> queryParameters = new Dictionary<string, string>();

            string queryString = GetQueryString(httpData);

            if (queryString == string.Empty)
                return queryParameters;

            string[] parameters = queryString.Split('&');

            foreach (string parameter in parameters)
            {
                string[] pair = parameter.Split('=');

                if(pair.Length != 2)
                    throw new HttpParseException("Wrong queries' structure");
                if (queryParameters.ContainsKey(pair[0]) == true)
                    throw new HttpParseException("Query parameter with the same key already exists");


                queryParameters.Add(pair[0], pair[1]);
            }

            return queryParameters;
        }

        public static IDictionary<string, string> GetCookieDictionary(string cookieData)
        {
            if (cookieData == null)
                throw new ArgumentNullException($"Parameter {nameof(cookieData)} must not be null");

            IDictionary<string, string> pairs = new Dictionary<string, string>();

            cookieData = cookieData.Replace(" ", "");

            string[] pairsString = cookieData.Split(';');

            foreach (var pairString in pairsString)
            {
                string[] pair = pairString.Split('=');

                if (pair.Length != 2)
                    throw new HttpParseException("Wrong cookie structure");

                pairs.Add(pair[0], pair[1]);
            }

            return pairs;
        }

        private static string GetUrl(string httpData)
        {
            Regex pathRegex = new Regex(@"(?<=^\w+\s).+(?= )", RegexOptions.IgnoreCase);

            string url = pathRegex.Match(httpData).Value
                .ToLower();

            if (url.Length == 0)
                throw new HttpParseException("Empty url");

            return url;
        }
    }
}