using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http;
using WebServer.Http.Interfaces;

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
                data = CreateResponse(data);

                byte[] messsage = Encoding.ASCII.GetBytes(data);
                stream.Write(messsage);
            }

            client.Close();
        }

        private static string CreateResponse(string data)
        {
            IHttpRequest request = new HttpRequest(data);
            IHttpResponse response = new HttpResponse();

            response.Cookie.Add("id", "123");

            response.Content = "<h1>Welcom to my server</h1>";

            return response.ToString();
        }
    }
}