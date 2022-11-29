using System.Linq;
using WebServer.Models.CookieCollections;

namespace WebServer.Services.HttpContextFactories.Helpers
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
