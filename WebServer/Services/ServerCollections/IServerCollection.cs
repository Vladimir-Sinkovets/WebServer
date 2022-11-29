using System.Collections.Generic;
using WebServer.Services.Servers;

namespace WebServer.Services.ServerCollections
{
    public interface IServerCollection : IEnumerable<IServer>
    {
        IServer GetServer(string name);
    }
}
