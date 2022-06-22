using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WebServer.Http.Interfaces;
using WebServer.Interfaces;
using WebServer.MyThreadPools;
using WebServer.OptionsModels;
using WebServer.ThreadPools;

namespace WebServer
{
    public class WebServer : IServer
    {
        private ITcpListener _server;
        private IClientHandler _clientHandler;
        private bool _isRunning = false;
        private IThreadPool _threadPool;
        private Thread _mainThread;


        public WebServer(ITcpListener listener, IServiceProvider serviceProvider, Action<IHttpContext> requestHandler, int threadsCount = 10)
        {
            _server = listener;
            _clientHandler = new ClientHandler(serviceProvider, requestHandler);
            _threadPool = new MyThreadPool(threadsCount);
        }

        public void Run()
        {
            _isRunning = true;

            _mainThread = new Thread(ListenClients);

            _mainThread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _server.Stop();
            _threadPool.Dispose();
            _mainThread.Interrupt();

            Console.WriteLine("Server has stopped");
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

                    _threadPool.Execute(HandleClient, client);
                }
            }
            catch (ThreadInterruptedException ex)
            {
                //Console.WriteLine();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
            }
            finally
            {
                _server.Stop();
            }
        }

        private void HandleClient(object state)
        {
            TcpClient client = (TcpClient)state;

            _clientHandler.Handle(new TcpClientAdapter(client));
        }

        public static IServer CreateServer<TStartUp>() where TStartUp : IStartUp, new()
        {
            IServiceCollection services = new ServiceCollection();

            IStartUp startUp = new TStartUp();

            startUp.ConfigureServices(services);
            AddWebServerConfigFile(services);

            return InstantiateWebServer(services, startUp);
        }

        private static IServer InstantiateWebServer(IServiceCollection services, IStartUp startUp)
        {
            IServiceProvider provider = services.BuildServiceProvider();

            WebServerConfiguration webConfig = provider.GetService<IOptions<WebServerConfiguration>>().Value;

            var ip = IPAddress.Parse(webConfig.IpAdress);
            var port = webConfig.Port;
            var threadsCount = webConfig.ThreadsCount;

            ITcpListener listener = new TcpListenerAdapter(new TcpListener(ip, port));

            return new WebServer(listener, provider, startUp.Handle, threadsCount);
        }

        private static void AddWebServerConfigFile(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.Configure<WebServerConfiguration>(configuration.GetSection("WebServerSettings"));
        }
    }
}