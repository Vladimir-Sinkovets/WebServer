﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Http
{
    public class ResponseCookieCollection : IResponseCookieCollection
    {
        private IDictionary<string, string> _pairs = new Dictionary<string, string>();

        public void Add(string key, string value) => _pairs.Add(key, value);

        public void Remove(string key) =>_pairs.Remove(key);

        public bool ContainValue(string value) => _pairs.Any(p => p.Value == value);

        public bool ContainHeader(string header) => _pairs.ContainsKey(header);

        public bool TryGetValue(string key, out string value)
        {
            bool result = _pairs.TryGetValue(key, out string pairValue);

            value = pairValue;

            return result;
        }
        
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }

        public override string ToString()
        {
            if (_pairs.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return _pairs
                    .Select(p => $"{p.Key}={p.Value}")
                    .Aggregate((x, y) => x + y);
            }
        }
    }
}
