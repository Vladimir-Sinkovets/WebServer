using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using WebServer.Http.Interfaces;
using WebServer.Http.Helpers;
using WebServer.Http.Models;
using WebServer.Http.Exceptions;
using WebServer.Services.TcpListenerFactories;

namespace WebServer.Services.ClientHandlers
{
    public class ClientHandler : IClientHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public Action<IHttpContext> RequestHandler { get; set; }


        public ClientHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public void Handle(ITcpClient client)
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();

                while (client.Connected == true)
                {
                    using IServiceScope scope = _serviceProvider.CreateScope();

                    byte[] data = ReadRequest(client, stream);

                    IHttpContext context = CreateHttpContext(data, scope);

                    RequestHandler.Invoke(context);

                    SendRequest(stream, context);
                }
            }
            catch (HttpParseException ex)
            {
                if (client.Connected == true)
                {
                    stream.Write(Encoding.ASCII.GetBytes("HTTP/1.1 400 Bad Request"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка : " + ex.ToString());

                if (client.Connected == true)
                {
                    stream.Write(Encoding.ASCII.GetBytes("HTTP/1.1 500 Internal Server Error"));
                }
            }
            finally
            {
                client.Close();
            }
        }


        private IHttpContext CreateHttpContext(byte[] data, IServiceScope scope)
        {
            int headDataLength = GetEndOfHeaderPosition(data);

            string headData = Encoding.UTF8.GetString(data, 0, headDataLength);

            byte[] body = data
                .Skip(headDataLength)
                .ToArray();

            IHttpRequest request = new HttpRequest(headData, body);
            IHttpResponse response = new HttpResponse();

            return new HttpContext(request, response, scope.ServiceProvider);
        }

        private byte[] ReadRequest(ITcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];

            int requestLength = stream.Read(bytes, 0, bytes.Length);

            return bytes
                .Take(requestLength)
                .ToArray();
        }

        private int GetEndOfHeaderPosition(byte[] bytes)
        {
            int i = 0;

            while (!((char)bytes[i + 1] == '\r' &&
                   (char)bytes[i + 2] == '\n' &&
                   (char)bytes[i + 3] == '\r' &&
                   (char)bytes[i + 4] == '\n'))
            {
                i++;
            }

            return i + 1;
        }

        private void SendRequest(NetworkStream stream, IHttpContext context)
        {
            stream.Write(context.Response.BuildResponseMessage());
        }
    }
}