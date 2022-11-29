using System.Collections.Generic;

namespace WebServer.Models.CookieCollections
{
    public interface IRequestCookieCollection : IEnumerable<KeyValuePair<string, string>>
    {
        bool ContainsHeader(string key);
        bool TryGetValue(string key, out string value);
    }
}
