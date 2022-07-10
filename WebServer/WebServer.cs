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
        private Thread _mainThread;
        private readonly ITcpListener _listener;
        private readonly IThreadPool _threadPool;
        private readonly IClientHandler _clientHandler;

        private readonly WebServerConfiguration _options;

        private bool _isRunning = false;

        public string Name { get; set; }

        public WebServer(IOptions<WebServerConfiguration> options, IClientHandler clientHandler)
        {
            _options = options.Value;

            Name = _options.Name;

            _listener = new TcpListenerAdapter(new TcpListener(IPAddress.Parse(_options.IpAddress), _options.Port));

            _clientHandler = new ClientHandler(DIContainer.Provider);

            _threadPool = new MyThreadPool(_options.ThreadsCount);
        }

        public void Run()
        {
            if (_clientHandler.RequestHandler == null)
                throw new Exception($"Method \"{nameof(SetHandler)}\" must be called before \"{nameof(Run)}\" method");

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

        public void SetHandler(Action<IHttpContext> action)
        {
            _clientHandler.RequestHandler = action;
        }


        private void ListenClients()
        {
            try
            {
                _listener.Start();

                while (_isRunning)
                {
                    Console.WriteLine("Waiting for connection...");

                    TcpClient client = _listener.AcceptTcpClient();

                    Console.WriteLine("Connected!");

                    _threadPool.Execute(HandleClient, client);
                }
            }
            catch (ThreadInterruptedException ex)
            {

            }
            catch (Exception ex)
            {

            }
            finally
            {
                _listener.Stop();
            }
        }

        private void HandleClient(object state)
        {
            TcpClient client = (TcpClient)state;

            _clientHandler.Handle(new TcpClientAdapter(client));
        }
    }
}