using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebServer
{
    public class WebServer : IServer
    {
        private TcpListener _server;
        private IClientHandler _clientHandler;
        private bool _isRunning = false;
        
        public WebServer(IPAddress ip, int port, IClientHandler clientHandler)
        {
            _server = new TcpListener(ip, port);
            _clientHandler = clientHandler;
        }

        public void Run()
        {
            _isRunning = true;
            Task.Run(() =>
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
                finally
                {
                    _server.Stop();
                }
            });
        }

        private void HandleClient(object state)
        {
            TcpClient client = (TcpClient)state;

            _clientHandler.Handle(client);
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}