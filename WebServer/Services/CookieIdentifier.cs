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
        private int _lastId = 0;
        public void SetId(IHttpContext context)
        {
            if (context.Request.Cookie.TryGetValue("id", out var id) == false)
            {
                lock (this)
                {
                    context.Response.Cookie.Add("id", $"{_lastId}");

                    _lastId++;
                }
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
