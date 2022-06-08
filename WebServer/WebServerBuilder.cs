using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Sockets;
using WebServer.Http.Interfaces;

namespace WebServer
{
    public class WebServerBuilder : IWebServerBuilder
    {
        private Action<IServiceCollection> _serviceCollectionConfiguring;
        private Action<IHttpContext> _requestHandler;
        private TcpListener _listener;

        public IServer Build()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            _serviceCollectionConfiguring.Invoke(serviceCollection);
            
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IServer server = new WebServer(_listener, serviceProvider, _requestHandler);

            return server;
        }

        public IWebServerBuilder ConfigureServices(Action<IServiceCollection> action)
        {
            _serviceCollectionConfiguring = action;
            return this;
        }

        public IWebServerBuilder SetHandler(Action<IHttpContext> action)
        {
            _requestHandler = action;
            return this;
        }

        public IWebServerBuilder SetListener(TcpListener listener)
        {
            _listener = listener;
            return this;
        }

        public static IWebServerBuilder CreateDefault() => new WebServerBuilder();
    }
}
