using System;
using WebServer.Http.Interfaces;

namespace WebServer.Services.Servers
{
    public interface IServer
    {
        void Run();
        void Stop();
        string Name { get; }
    }
}