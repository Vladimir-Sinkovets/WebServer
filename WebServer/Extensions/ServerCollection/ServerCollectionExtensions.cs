using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Interfaces;

namespace WebServer.Extensions.ServerCollection
{
    public static class ServerCollectionExtensions
    {
        public static IServer GetServerByName(this IServerCollection collection, string name)
        {
            return collection.FirstOrDefault(server => server.Name == name);
        }
    }
}
