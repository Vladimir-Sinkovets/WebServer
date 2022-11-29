using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Services.ClientHandlers;
using WebServer.Services.ServerCollections;
using WebServer.Services.Servers;
using WebServer.Services.Servers.OptionsModels;
using WebServer.Services.TcpListenerFactories;
using WebServer.Services.ThreadPools;

namespace WebServer.Extensions.ServiceCollectionEx
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServer(this IServiceCollection services, string sectionName)
        {
            return services
                .AddSingleton<IServer, Server>(sp =>
                {
                    ServerConfiguration serverConfig = new ServerConfiguration();

                    IConfiguration config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

                    var section = config.GetSection(sectionName)
                        .GetSection("WebServerSettings");

                    serverConfig.IpAddress = section["ipAddress"];
                    serverConfig.Port = int.Parse(section["port"]);
                    serverConfig.ThreadsCount = int.Parse(section["threadsCount"]);
                    serverConfig.Name = section["name"];

                    IOptions<ServerConfiguration> opt = Options.Create(serverConfig);
                    return new Server(
                        opt,
                        sp.GetService<IClientHandler>(),
                        sp.GetService<IThreadPool>(),
                        sp.GetService<ITcpListenerFactory>()
                        );
                });
        }

        public static IServiceCollection AddServers(this IServiceCollection services, params string[] serverSections)
        {
            services.AddSingleton<IServerCollection, ServerCollection>();

            foreach (var section in serverSections)
            {
                services.AddServer(section);
            }

            return services;
        }
    }
}
