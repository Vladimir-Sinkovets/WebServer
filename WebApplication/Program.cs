using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using WebServer;
using WebServer.Extensions.ServiceCollection;
using WebServer.Http.Interfaces;
using WebServer.Interfaces;
using WebServer.Services;

namespace WebApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServer server = ConfigureServer();
            server.Run();
        }

        private static void Handle(IHttpContext context)
        {
            context.Response.Body = Encoding.UTF8.GetBytes("Hello");
        }

        private static IServer ConfigureServer()
        {
            DIContainer.ConfigureServices(services =>
            {
                services.AddSingleton<IServerCollection, ServerCollection>();
                services.AddScoped<ICookieIdentifier, CookieIdentifier>();
                services.AddServer(sectionName: "server");
            });

            IServer server = DIContainer.GetService<IServerCollection>()
                .GetServer(name: "server");

            server.SetHandler(Handle);

            return server;
        }
    }
}
