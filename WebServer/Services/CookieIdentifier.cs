using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Services
{
    public class CookieIdentifier : ICookieIdentifier
    {
        private const string CookieHeaderName = "id";
        public Guid CurrentUserId { get; private set; }

        public Guid IdentifyUser(IHttpContext context)
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
        //public void SetCookie(IHttpContext context)
        //{
        //    if (context.Request.Cookie.TryGetValue(CookieHeaderName, out var id) == false)
        //    {
        //        context.Response.Cookie.Add(CookieHeaderName, $"{Guid.NewGuid()}");
        //    }
        //}

        //public bool TryGetCurrentClientId(IHttpContext context, out string id)
        //{
        //    bool result = context.Request.Cookie.TryGetValue(CookieHeaderName, out string cookieId);

        //    id = cookieId;

        //    return result;
        //}
    }
}
