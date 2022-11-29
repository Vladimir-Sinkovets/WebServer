using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading;
using WebServer.Services.ThreadPools;
using WebServer.Services.ClientHandlers;
using WebServer.Services.TcpListenerFactories;
using WebServer.Services.Servers.OptionsModels;

namespace WebServer.Services.Servers
{
    public class Server : IServer
    {
        private Thread _mainThread;
        private readonly ITcpListener _listener;
        private readonly IThreadPool _threadPool;
        private readonly IClientHandler _clientHandler;

        private readonly ServerConfiguration _options;

        private bool _isRunning = false;

        public string Name { get; }

        public Server(IOptions<ServerConfiguration> options, IClientHandler clientHandler,
            IThreadPool threadPool, ITcpListenerFactory tcpListener)
        {
            _options = options.Value;

            Name = _options.Name;

            _listener = tcpListener.GetInstance(IPAddress.Parse(_options.IpAddress), _options.Port);

            _clientHandler = clientHandler;

            _threadPool = threadPool;
        }

        public void Run()
        {
            // rewrite : 
            //if (_clientHandler.RequestHandler == null)
            //    throw new Exception($"Method \"{nameof(SetHandler)}\" must be called before \"{nameof(Run)}\" method"); // rewrite

            _isRunning = true;

            _mainThread = new Thread(ListenClients);

            _mainThread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            _threadPool.Dispose();
            _mainThread.Interrupt();

            Console.WriteLine("Server has stopped");
        }


        private void ListenClients()
        {
            try
            {
                _listener.Start();

                while (_isRunning)
                {
                    Console.WriteLine("Waiting for connection...");

                    ITcpClient client = _listener.AcceptTcpClient();

                    Console.WriteLine("Connected!");

                    _threadPool.Execute(HandleClient, client);
                }
            }
            catch (ThreadInterruptedException)
            {
                // add logger mb
            }
            catch (Exception)
            {
                // add logger mb
            }
            finally
            {
                _listener.Stop();
            }
        }

        private void HandleClient(object state)
        {
            ITcpClient client = (ITcpClient)state;

            _clientHandler.Handle(client);
        }
    }
}