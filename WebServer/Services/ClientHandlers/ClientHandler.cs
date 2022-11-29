using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using WebServer.Services.TcpListenerFactories;
using WebServer.Models;
using WebServer.Services.HttpContextFactories.Exceptions;
using WebServer.Services.HttpContextFactories.Helpers;
using WebServer.Services.HttpContextFactories;

namespace WebServer.Services.ClientHandlers
{
    public class ClientHandler : IClientHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextFactory _httpContextFactory;

        public Action<HttpContext> RequestHandler { get; set; }


        public ClientHandler(IServiceProvider serviceProvider, IHttpContextFactory httpContextFactory)
        {
            _serviceProvider = serviceProvider;
            _httpContextFactory = httpContextFactory;
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

                    HttpContext context = _httpContextFactory.CreateInstance(data, scope.ServiceProvider);

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

        private static byte[] ReadRequest(ITcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];

            int requestLength = stream.Read(bytes, 0, bytes.Length);

            return bytes
                .Take(requestLength)
                .ToArray();
        }


        private static void SendRequest(NetworkStream stream, HttpContext context)
        {
            stream.Write(context.Response.BuildResponseMessage());
        }
    }
}