using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Interfaces;

namespace WebServer
{
    public class ServerCollection : IServerCollection
    {
        private IEnumerable<IServer> _servers;

        public ServerCollection(IEnumerable<IServer> servers)
        {
            _servers = servers;
        }

        public IEnumerator<IServer> GetEnumerator() => _servers.GetEnumerator();

        public IServer GetServer(string name) => _servers.FirstOrDefault(s => s.Name == name);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
