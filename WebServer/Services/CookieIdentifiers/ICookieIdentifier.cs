using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Services.CookieIdentifiers
{
    public interface ICookieIdentifier
    {
        public Guid CurrentUserId { get; }
        public Guid IdentifyUser(IHttpContext context);
    }
}
