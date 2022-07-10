using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Interfaces;
using WebServer.OptionsModels;

namespace WebServer.Extensions.ServiceCollectionEx
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServer(this IServiceCollection services, string sectionName)
        {
            return services
                .AddSingleton<IServer, WebServer>(sp =>
                {
                    WebServerConfiguration serverConfig = new WebServerConfiguration();

                    IConfiguration config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

                    var section = config.GetSection(sectionName)
                        .GetSection("WebServerSettings");

                    serverConfig.IpAddress = section["ipAddress"];
                    serverConfig.Port = int.Parse(section["port"]);
                    serverConfig.ThreadsCount = int.Parse(section["threadsCount"]);
                    serverConfig.Name = section["name"];

                    IOptions<WebServerConfiguration> opt = Options.Create(serverConfig);
                    return new WebServer(opt, DIContainer.GetService<IClientHandler>());
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
