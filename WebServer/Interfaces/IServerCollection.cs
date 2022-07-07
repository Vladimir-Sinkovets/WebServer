using System.Collections.Generic;

namespace WebServer.Interfaces
{
    public interface IServerCollection : IEnumerable<IServer>
    {
        IServer GetServer(string name);
    }
}
