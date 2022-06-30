using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Interfaces
{
    public interface IClientHandler
    {
        void Handle(ITcpClient client);
        Action<IHttpContext> RequestHandler { get; set; }
    }
}
