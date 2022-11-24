﻿using System;
using WebServer.Http.Interfaces;
using WebServer.Services.TcpListenerFactories;

namespace WebServer.Services.ClientHandlers
{
    public interface IClientHandler
    {
        void Handle(ITcpClient client);
        Action<IHttpContext> RequestHandler { get; set; }
    }
}
