using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Services.Http.CookieCollections;

namespace WebServer.Services.Http.Helpers
{
    internal static class IResponseCookieCollectionExtentions
    {
        public static string ConvertToString(this IResponseCookieCollection cookie)
        {
            if (!cookie.Any())
            {
                return string.Empty;
            }
            else
            {
                return cookie
                    .Select(p => $"{p.Key}={p.Value}")
                    .Aggregate((x, y) => x + y);
            }
        }
    }
}
