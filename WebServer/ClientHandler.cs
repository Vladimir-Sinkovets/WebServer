using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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
            _requestHandler = requestHandler;
        }

        public void Handle(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            while (client.Connected)
            {
                byte[] data = ReadRequest(client, stream);

                int headDataLength = GetEndOfHeaderPosition(data);

                string headData = Encoding.ASCII.GetString(data, 0, headDataLength);

                byte[] body = data
                    .Skip(headDataLength)
                    .ToArray();

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IHttpContext context = new HttpContext(headData, body, scope.ServiceProvider);

                    _requestHandler.Invoke(context);

                    SendData(stream, context);
                }
            }

            client.Close();
        }

        private static byte[] ReadRequest(TcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            int requestLength = stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        private static int GetEndOfHeaderPosition(byte[] bytes)
        {
            int headDataLength = 0;

            while (!((char)bytes[headDataLength + 1] == '\r' &&
                   (char)bytes[headDataLength + 2] == '\n' &&
                   (char)bytes[headDataLength + 3] == '\r' &&
                   (char)bytes[headDataLength + 4] == '\n'))
            {
                headDataLength++;
            }

            return headDataLength + 1;
        }

        //public void Handle(TcpClient client)
        //{
        //    NetworkStream stream = client.GetStream();

        //    while (client.Connected)
        //    {
        //        byte[] bytes = new byte[client.ReceiveBufferSize];
        //        int requestLength = stream.Read(bytes, 0, bytes.Length);



        //        string data = Encoding.ASCII.GetString(bytes, 0, requestLength);

        //        using (IServiceScope scope = _serviceProvider.CreateScope())
        //        {
        //            IHttpContext context = new HttpContext(data, scope.ServiceProvider);

        //            _requestHandler.Invoke(context);

        //            SendData(stream, context);
        //        }
        //    }

        //    client.Close();
        //}

        private void SendData(NetworkStream stream, IHttpContext context)
        {
            string data = context.Response.ToString();

            byte[] messsage = Encoding.ASCII.GetBytes(data);

            stream.Write(messsage);
        }
    }
}