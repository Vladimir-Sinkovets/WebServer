using System;
using WebServer.Models;

namespace WebServer.Services.CookieIdentifiers
{
    public interface ICookieIdentifier
    {
        public Guid CurrentUserId { get; }
        public Guid IdentifyUser(HttpContext context);
    }
}
