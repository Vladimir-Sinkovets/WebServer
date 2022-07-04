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

namespace WebServer.Extensions.ServiceCollection
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
                    return new WebServer(opt);
                });
        }
    }
}
