using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WebServer.Http.Interfaces;
using WebServer.Interfaces;
using WebServer.OptionsModels;
using WebServer.Services;

namespace WebServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IServer server = WebServer.CreateServer<DefaultStartUp>(); 


            server.Run();

            Thread.Sleep(10000);

            server.Stop();
        }

    }
}