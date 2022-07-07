using System;
using WebServer.Http.Interfaces;

namespace WebServer.Interfaces
{
    public interface IClientHandler
    {
        void Handle(ITcpClient client);
        Action<IHttpContext> RequestHandler { get; set; }
    }
}
