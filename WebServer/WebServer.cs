using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WebServer.Http.Interfaces;
using WebServer.Interfaces;
using WebServer.OptionsModels;

namespace WebServer
{
    public class WebServer : IServer
    {
        private ITcpListener _server;
        private IClientHandler _clientHandler;
        private bool _isRunning = false;

        public WebServer(ITcpListener listener, IServiceProvider serviceProvider, Action<IHttpContext> requestHandler)
        {
            _server = listener;
            _clientHandler = new ClientHandler(serviceProvider, requestHandler);
        }

        public void Run()
        {
            _isRunning = true;

            Thread thread = new Thread(ListenClients);

            thread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void ListenClients()
        {
            try
            {
                _server.Start();

                while (_isRunning)
                {
                    Console.WriteLine("Waiting for connection...");

                    TcpClient client = _server.AcceptTcpClient();

                    Console.WriteLine("Connected!");

                    ThreadPool.QueueUserWorkItem(HandleClient, client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            {
                _server.Stop();
            }
        }

        private void HandleClient(object state)
        {
            TcpClient client = (TcpClient)state;

            _clientHandler.Handle(new TcpClientAdapter(client));
        }

        public static IServer CreateServer<T>() where T : IStartUp, new()
        {
            IServiceCollection services = new ServiceCollection();
            IStartUp startUp = new T();

            startUp.ConfigureServices(services);
            IServiceProvider provider = services.BuildServiceProvider();

            WebServerConfiguration configuration = provider.GetService<IOptions<WebServerConfiguration>>().Value;

            IPAddress ip = IPAddress.Parse(configuration.IpAdress);
            int port = configuration.Port;

            ITcpListener listener = new TcpListenerAdapter(new TcpListener(ip, port));

            return new WebServer(listener, provider, startUp.Handle);
        }

    }
}