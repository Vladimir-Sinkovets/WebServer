using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http;
using WebServer.Http.Interfaces;
using WebServer.Services;

namespace WebServer
{
    class ClientHandler : IClientHandler
    {
        private ICookieIdentifier _identifier = new CookieIdentifier();

        public void Handle(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            while (client.Connected)
            {
                byte[] bytes = new byte[4096];
                int i = stream.Read(bytes, 0, bytes.Length);
                string data = Encoding.ASCII.GetString(bytes, 0, i);

                IHttpContext context = new HttpContext(data);
                data = CreateResponse(context);

                byte[] messsage = Encoding.ASCII.GetBytes(data);
                stream.Write(messsage);
            }

            client.Close();
        }

        private string CreateResponse(IHttpContext context)
        {
            _identifier.SetId(context);

            bool result = _identifier.TryGetCurrentClientId(context, out string id);

            string message;

            if (result == true)
            {
                message = $"Your id = {id}";
            }
            else
            {
                message = $"Hi, new client";
            }

            context.Response.Content = $"<h1>Welcom to my server. {message}</h1>";

            return context.Response.ToString();
        }
    }
}