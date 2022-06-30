using System;
using WebServer.Http.Interfaces;

namespace WebServer.Interfaces
{
    public interface IServer
    {
        void Run();
        void Stop();
        void SetHandler(Action<IHttpContext> action);
    }
}