using System;
using System.Net.Sockets;
using System.Threading;
using WebServer.Http.Interfaces;

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

        public void Stop()
        {
            _isRunning = false;
        }
    }
}