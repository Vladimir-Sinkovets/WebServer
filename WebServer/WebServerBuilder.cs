using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using WebServer.Http.Interfaces;
using WebServer.Interfaces;
using WebServer.OptionsModels;

namespace WebServer
{
    public class WebServerBuilder : IWebServerBuilder
    {
        private ICollection<Action<IServiceCollection>> _serviceCollectionConfigurings = new List<Action<IServiceCollection>>();
        private Action<IHttpContext> _requestHandler;
        private ITcpListener _listener;

        private WebServerBuilder()
        {

        }

        public IServer Build()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            foreach (var item in _serviceCollectionConfigurings)
            {
                item.Invoke(serviceCollection);
            }

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IServer server = new WebServer(_listener, serviceProvider, _requestHandler);

            return server;
        }

        public IWebServerBuilder ConfigureServices(Action<IServiceCollection> action)
        {
            _serviceCollectionConfigurings.Add(action);
            return this;
        }

        public IWebServerBuilder SetHandler(Action<IHttpContext> action)
        {
            _requestHandler = action;
            return this;
        }

        public IWebServerBuilder SetListener(ITcpListener listener)
        {
            _listener = listener;
            return this;
        }

        public static IWebServerBuilder CreateDefaultBuider()
        {
            IWebServerBuilder builder = new WebServerBuilder();

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();

            builder.ConfigureServices(services => services.Configure<WebServerConfiguration>(configuration.GetSection("WebServerSettings")));

            return builder;
        }
    }
}
