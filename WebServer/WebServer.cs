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

        public string Name { get; set; }
        public Action<IHttpContext> Handler { get; set; }

        public WebServer(IOptions<WebServerConfiguration> options)
        {
            WebServerConfiguration configuration = options.Value;

            _server = new TcpListenerAdapter(new TcpListener(IPAddress.Parse(configuration.IpAdress), configuration.Port));

            _clientHandler = new ClientHandler(DIContainer.Provider);

            _threadPool = new MyThreadPool(configuration.ThreadsCount);
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

        public void SetHandler(Action<IHttpContext> action)
        {
            _clientHandler.RequestHandler = action;
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

    }
}