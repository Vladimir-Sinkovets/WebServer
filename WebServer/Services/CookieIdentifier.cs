using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Services
{
    internal class CookieIdentifier : ICookieIdentifier
    {
        public void SetId(IHttpContext context)
        {
            if (context.Request.Cookie.TryGetValue("id", out var id) == false)
            {
                context.Response.Cookie.Add("id", $"{Guid.NewGuid()}");
            }
        }

        public bool TryGetCurrentClientId(IHttpContext context, out string id)
        {
            bool result = context.Request.Cookie.TryGetValue("id", out string cookieId);

            id = cookieId;

            return result;
        }
    }
}
