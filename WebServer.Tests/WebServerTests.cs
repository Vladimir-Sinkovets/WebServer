using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServer.Interfaces;
using Xunit;

namespace WebServer.Tests
{
    public class WebServerTests
    {
        [Fact]
        public void Should_Send200StatusCodeResponseToClient()
        {
            // Arrange
            string ip = "127.0.0.2";
            int port = 8887;
            string request = "GET 127.0.0.2:8887/index HTTP/1.1\n\r" +
                "Cookie: id=a3fWa\r\n\r\n";

            ITcpListener listener= new TcpListenerAdapter(new TcpListener(IPAddress.Parse(ip), port));
            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();
            IServer server = new WebServer(listener, provider, context => { }, 1);

            server.Run();

            // Act
            byte[] response = GetResponseByServer(ip, port, request);
            string message = Encoding.UTF8.GetString(response, 0, response.Length);
            server.Stop();

            // Assert
            message.ToLower().Should().Contain("http/1.1 200 ok");
        }

        [Fact]
        public void Should_SendBodyInResponseToClient()
        {
            // Arrange
            string ip = "127.0.0.2";
            int port = 8887;
            string request = "GET 127.0.0.2:8887/index HTTP/1.1\n\r" +
                "Cookie: id=a3fWa\r\n\r\n";

            ITcpListener listener = new TcpListenerAdapter(new TcpListener(IPAddress.Parse(ip), port));
            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();
            IServer server = new WebServer(listener, provider, context => 
            {
                context.Response.Body = Encoding.UTF8.GetBytes("Hello world!");
            }
            , 1);


            // Act
            server.Run();

            byte[] response = GetResponseByServer(ip, port, request);
            string message = Encoding.UTF8.GetString(response, 0, response.Length);
            
            server.Stop();

            // Assert
            message.ToLower().Should().Contain("http/1.1 200 ok");
            message.ToLower().Should().Contain("hello world!");
        }

        [Fact]
        public void Should_Send400StatusCodeInResponseToClient_WhenRequestHasWrongSyntax()
        {
            // Arrange
            string ip = "127.0.0.2";
            int port = 8887;
            string request = "GT 127.0.0.2:8887/index HTTP/1.1\n\r" +
                "Cookie: id=a3fWa\r\n\r\n";

            ITcpListener listener = new TcpListenerAdapter(new TcpListener(IPAddress.Parse(ip), port));
            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();
            IServer server = new WebServer(listener, provider, context => { }, 1);


            // Act
            server.Run();

            byte[] response = GetResponseByServer(ip, port, request);
            string message = Encoding.UTF8.GetString(response, 0, response.Length);
            
            server.Stop();

            // Assert
            message.ToLower().Should().Contain("http/1.1 400 bad request");
        }

        [Fact]
        public void Should_Send500StatusCodeInResponseToClient_WhenHandlerThrowExeception()
        {
            // Arrange
            string ip = "127.0.0.2";
            int port = 8887;
            string request = "GeT 127.0.0.2:8887/index HTTP/1.1\n\r" +
                "Cookie: id=a3fWa\r\n\r\n";

            ITcpListener listener = new TcpListenerAdapter(new TcpListener(IPAddress.Parse(ip), port));
            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();
            IServer server = new WebServer(listener, provider, context => 
            {
                throw new Exception("test exception");
            }, 1);


            // Act
            server.Run();

            byte[] response = GetResponseByServer(ip, port, request);
            string message = Encoding.UTF8.GetString(response, 0, response.Length);

            server.Stop();

            // Assert
            message.ToLower().Should().Contain("http/1.1 500 internal server error");

        }

        [Fact]
        public void Should_HandleClientsWithMultiThreading()
        {
            int clientsCount = 10;
            // Arrange
            string ip = "127.0.0.2";
            int port = 8887;
            string request = "GeT 127.0.0.2:8887/index HTTP/1.1\n\r" +
                "Cookie: id=a3fWa\r\n\r\n";

            ITcpListener listener = new TcpListenerAdapter(new TcpListener(IPAddress.Parse(ip), port));
            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();
            IServer server = new WebServer(listener, provider, context =>
            {
                Thread.Sleep(100);
            }, 5);


            // Act
            server.Run();

            IList<string> responses = new List<string>();
            for (int i = 0; i < clientsCount; i++)
            {
                TcpClient client = new TcpClient();
                
                Parameters state = new Parameters()
                { 
                    Port = port,
                    Ip = ip,
                    Request = request,
                };
                
                Thread thread = new Thread(state => 
                {
                    Parameters parameters = (Parameters)state;

                    byte[] bytes = GetResponseByServer(parameters.Ip, parameters.Port, parameters.Request);
                    string message = Encoding.UTF8.GetString(bytes);

                    lock (this)
                    {
                        responses.Add(message);
                    }

                });
                thread.Start(state);
            }
            Thread.Sleep(2000);
            server.Stop();

            // Assert

            responses.Count.Should().Be(clientsCount);
        }
        class Parameters
        {
            public int Port{ get; set; }
            public string Ip { get; set; }
            public string Request { get; set; }
            public TcpClient Client { get; set; }
        }


        private byte[] GetResponseByServer(string ip, int port, string request)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);

            NetworkStream stream = tcpClient.GetStream();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(request);

            stream.Write(data, 0, data.Length);

            byte[] data2 = new byte[tcpClient.ReceiveBufferSize];
            int bytes = stream.Read(data2, 0, data2.Length);

            byte[] response = data2[..bytes];

            tcpClient.Close();

            return response;
        }

        private byte[] GetResponseByServer(string ip, int port, string request, TcpClient tcpClient)
        {
            tcpClient.Connect(ip, port);

            NetworkStream stream = tcpClient.GetStream();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(request);

            stream.Write(data, 0, data.Length);

            byte[] data2 = new byte[tcpClient.ReceiveBufferSize];
            int bytes = stream.Read(data2, 0, data2.Length);

            byte[] response = data2[..bytes];

            return response;
        }
    }
}
