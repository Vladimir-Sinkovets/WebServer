using System.Collections.Generic;

namespace WebServer.Models.CookieCollections
{
    public interface IResponseCookieCollection : IEnumerable<KeyValuePair<string, string>>
    {
        void Add(string key, string value);
        void Remove(string key);
        bool ContainHeader(string header);
        bool ContainValue(string value);
        bool TryGetValue(string key, out string value);
    }
}
