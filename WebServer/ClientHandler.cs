using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Sockets;
using System.Text;
using WebServer.Http;
using WebServer.Http.Interfaces;
using WebServer.Services;

namespace WebServer
{
    class ClientHandler : IClientHandler
    {
        private IServiceProvider _serviceProvider;

        private readonly Action<IHttpContext> _requestHandler;

        public ClientHandler(IServiceProvider serviceProvider, Action<IHttpContext> requestHandler)
        {
            _serviceProvider = serviceProvider;
            this._requestHandler = requestHandler;
        }

        public void Handle(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            while (client.Connected)
            {
                byte[] bytes = new byte[client.ReceiveBufferSize];
                int i = stream.Read(bytes, 0, bytes.Length);

                string data = Encoding.ASCII.GetString(bytes, 0, i);

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IHttpContext context = new HttpContext(data, scope.ServiceProvider);

                    _requestHandler.Invoke(context);

                    SendData(stream, context);
                }
            }

            client.Close();
        }

        private void SendData(NetworkStream stream, IHttpContext context)
        {
            string data = context.Response.ToString();

            byte[] messsage = Encoding.ASCII.GetBytes(data);

            stream.Write(messsage);
        }
    }
}