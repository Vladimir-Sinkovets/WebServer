using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Services.Servers;

namespace WebServer.Services.ServerCollections
{
    public class ServerCollection : IServerCollection
    {
        private readonly IEnumerable<IServer> _servers;

        public ServerCollection(IEnumerable<IServer> servers)
        {
            _servers = servers;
        }

        public IServer GetServer(string name) => _servers.FirstOrDefault(s => s.Name == name);

        public IEnumerator<IServer> GetEnumerator() => _servers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}