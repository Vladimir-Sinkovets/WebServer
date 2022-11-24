using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WebServer.Http.Interfaces;
using WebServer.OptionsModels;
using WebServer.Extensions.ServerCollectionEx;
using WebServer.Extensions.ServiceCollectionEx;
using WebServer.Services.CookieIdentifiers;
using WebServer.Services.ServerCollections;
using WebServer.Services.Servers;

namespace WebServer
{
    internal class Program
    {
        static void Main()
        {
            IServer server = ConfigureServer();
            server.Run();
        }

        private static void Handle(IHttpContext context)
        {
            ICookieIdentifier identifier = context.ServiceProvider.GetService<ICookieIdentifier>();

            identifier.IdentifyUser(context);

            context.Response.StatusCode = Enums.StatusCode.OK;
            context.Response.ContentType = null;

            context.Response.Body = Encoding.ASCII.GetBytes($"Hello world!");
        }

        private static IServer ConfigureServer()
        {
            DIContainer.ConfigureServices(services =>
            {
                services.AddSingleton<IServerCollection, ServerCollection>();
                services.AddServers(new string[] { "Server_1", });
                services.AddScoped<ICookieIdentifier, CookieIdentifier>();
            });

            IServer server = DIContainer.GetService<IServerCollection>()
                .GetServer(name: "server_1");
            server.SetHandler(Handle);
            return server;
        }
    }
}