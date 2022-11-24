using FluentAssertions;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServer.OptionsModels;
using WebServer.Services.Servers;
using Xunit;

namespace WebServer.Tests
{
    public class WebServerTests
    {
        public WebServerTests()
        {
            DIContainer.ConfigureServices(s => { });
        }
        [Fact]
        public void Should_Send200StatusCodeResponseToClient()
        {
            // Arrange
            string ip = "127.0.0.2";
            int port = 8887;
            string request = "GET 127.0.0.2:8887/index HTTP/1.1\n\r" +
                "Cookie: id=a3fWa\r\n\r\n";
            WebServerConfiguration config = new WebServerConfiguration()
            {
                IpAddress = "127.0.0.2",
                Port = 8887,
                ThreadsCount = 1,
                Name = "server,"
            };

            IOptions<WebServerConfiguration> option = Options.Create(config);

            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();

            IServer server = new WebServer(option, new ClientHandler(provider));
            server.SetHandler(c => { });

            server.Run();

            // Act
            byte[] response = GetResponseByServer(ip, port, request);
            string message = Encoding.UTF8.GetString(response, 0, response.Length);
            server.Stop();

            // Assert
            message.ToLower().Should().Contain("ok");
        }

        [Fact]
        public void Should_SendBodyInResponseToClient()
        {
            // Arrange
            string ip = "127.0.0.2";
            int port = 8887;
            string request = "GET 127.0.0.2:8887/index HTTP/1.1\n\r" +
                "Cookie: id=a3fWa\r\n\r\n";

            WebServerConfiguration config = new WebServerConfiguration()
            {
                IpAddress = ip,
                Port = port,
                ThreadsCount = 1,
                Name = "server,"
            };

            IOptions<WebServerConfiguration> options = Options.Create(config);

            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();

            IServer server = new WebServer(options, new ClientHandler(provider));
            server.SetHandler(context =>
            {
                context.Response.Body = Encoding.UTF8.GetBytes("Hello world!");
            });

            // Act
            server.Run();

            byte[] response = GetResponseByServer(ip, port, request);
            string message = Encoding.UTF8.GetString(response, 0, response.Length);
            
            server.Stop();

            // Assert
            message.ToLower().Should().Contain("ok");
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

            WebServerConfiguration config = new WebServerConfiguration()
            {
                IpAddress = ip,
                Port = port,
                ThreadsCount = 1,
                Name = "server,"
            };

            IOptions<WebServerConfiguration> options = Options.Create(config);

            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();
            
            IServer server = new WebServer(options, new ClientHandler(provider));
            server.SetHandler(context => { });


            // Act
            server.Run();

            byte[] response = GetResponseByServer(ip, port, request);
            string message = Encoding.UTF8.GetString(response, 0, response.Length);
            
            server.Stop();

            // Assert
            message.ToLower().Should().Contain("bad request");
        }

        [Fact]
        public void Should_Send500StatusCodeInResponseToClient_WhenHandlerThrowExeception()
        {
            // Arrange
            string ip = "127.0.0.2";
            int port = 8887;
            string request = "GeT 127.0.0.2:8887/index HTTP/1.1\n\r" +
                "Cookie: id=a3fWa\r\n\r\n";

            WebServerConfiguration config = new WebServerConfiguration()
            {
                IpAddress = ip,
                Port = port,
                ThreadsCount = 1,
                Name = "server,"
            };

            IOptions<WebServerConfiguration> options = Options.Create(config);

            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();

            IServer server = new WebServer(options, new ClientHandler(provider));
            server.SetHandler(context =>
            {
                throw new Exception("test exception");
            });

            // Act
            server.Run();

            byte[] response = GetResponseByServer(ip, port, request);
            string message = Encoding.UTF8.GetString(response, 0, response.Length);

            server.Stop();

            // Assert
            message.ToLower().Should().Contain("http/1.1 500 internal server error");

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
    }
}
