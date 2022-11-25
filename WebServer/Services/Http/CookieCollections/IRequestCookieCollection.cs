using System.Collections.Generic;

namespace WebServer.Services.Http.CookieCollections
{
    public interface IRequestCookieCollection : IEnumerable<KeyValuePair<string, string>>
    {
        bool ContainsHeader(string key);
        bool TryGetValue(string key, out string value);
    }
}
