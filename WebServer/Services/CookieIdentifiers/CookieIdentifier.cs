using System;
using WebServer.Models;

namespace WebServer.Services.CookieIdentifiers
{
    public class CookieIdentifier : ICookieIdentifier
    {
        private const string CookieHeaderName = "id";
        public Guid CurrentUserId { get; private set; }

        public Guid IdentifyUser(HttpContext context)
        {
            bool result = context.Request.Cookie.TryGetValue(CookieHeaderName, out string cookieId);

            if (result == true)
            {
                CurrentUserId = Guid.Parse(cookieId);
            }
            else
            {
                CurrentUserId = Guid.NewGuid();

                context.Response.Cookie.Add(CookieHeaderName, $"{CurrentUserId}");
            }

            return CurrentUserId;
        }
    }
}
