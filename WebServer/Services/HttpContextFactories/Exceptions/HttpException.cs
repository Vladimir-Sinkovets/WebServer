﻿using System;

namespace WebServer.Services.HttpContextFactories.Exceptions
{
    public class HttpException : Exception
    {
        public HttpException() { }
        public HttpException(string message) : base(message) { }
        public HttpException(string message, Exception inner) : base(message, inner) { }
    }
}
