using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class WebServer : IServer
    {
        private TcpListener server;
        
        public WebServer(IPAddress ip, int port)
        {
            server = new TcpListener(ip, port);
        }

        public void Run()
        {
            try
            {
                server.Start();
                byte[] bytes = new byte[4096];
                string data = null;

                while (true)
                {
                    Console.WriteLine("Waiting for connection...");
                    
                    TcpClient client = server.AcceptTcpClient();

                    Console.WriteLine("Connected!");

                    NetworkStream stream = client.GetStream();

                    while (client.Connected)
                    {
                        int i = stream.Read(bytes, 0, bytes.Length);
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        Console.WriteLine($"Received: {data}");
                        
                        data = "HTTP/1.1 200\nContent-Length:28\nContent-Type:text/html\nConnection:Closed\n\n<h1>Welcom to my server</h1>";

                        byte[] messsage = Encoding.ASCII.GetBytes(data);
                        stream.Write(messsage);

                        Console.WriteLine($"Sent: {data}");
                    }

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                server.Stop();
            }
        }

        public void Stop()
        {
            server.Stop();
        }
    }
}
