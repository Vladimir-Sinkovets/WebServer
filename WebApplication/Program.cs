using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using WebServer;
using WebServer.Extensions.ServiceCollectionEx;
using WebServer.Http.Interfaces;
using WebServer.Services.CookieIdentifiers;
using WebServer.Services.ServerCollections;
using WebServer.Services.Servers;

namespace WebApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServer server = ConfigureServer();
            server.Run();
        }

        private static void Handle(HttpContext context)
        {
            context.Response.Body = Encoding.UTF8.GetBytes("Hello");
        }

        private static IServer ConfigureServer()
        {
            DIContainer.ConfigureServices(services =>
            {
                services.AddServers(new string[] { "server", });
                services.AddScoped<ICookieIdentifier, CookieIdentifier>();
            });

            IServer server = DIContainer.GetService<IServerCollection>()
                .GetServer(name: "server");

            server.SetHandler(Handle);

            return server;
        }
    }
}
