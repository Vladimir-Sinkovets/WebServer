using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class ClientHandler : IClientHandler
    {
        public void Handle(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            while (client.Connected)
            {
                byte[] bytes = new byte[4096];
                int i = stream.Read(bytes, 0, bytes.Length);
                string data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine(data);
                HttpRequest request = new HttpRequest(data);
                HttpResponse response = new HttpResponse();

                response.AddHeader("Connection", "Closed");
                response.AddHeader("Set-Cookie", "id=a3fWa; Expires=Wed, 21 Oct 2026 07:28:00 GMT;");
                response.Content = "<h1>Welcom to my server</h1>";

                data = response.ToString();

                byte[] messsage = Encoding.ASCII.GetBytes(data);
                stream.Write(messsage);
            }

            client.Close();
        }
    }
}
