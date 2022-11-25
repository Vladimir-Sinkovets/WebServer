using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Services.Http.Exceptions
{
    public class HttpParseException : HttpException
    {
        public HttpParseException() { }
        public HttpParseException(string message) : base(message) { }
        public HttpParseException(string message, Exception inner) : base(message, inner) { }
    }
}
