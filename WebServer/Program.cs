using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            IServer server = new WebServer(ip, port);
            
            server.Run();
        }
    }
}
