using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Services.Http.Models;

namespace WebServer.Services.Http
{
    public interface IHttpContextFactory
    {
        HttpContext CreateInstance(byte[] data, IServiceProvider provider);
    }
}
