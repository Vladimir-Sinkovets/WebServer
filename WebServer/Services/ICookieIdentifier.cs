using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Http.Interfaces;

namespace WebServer.Services
{
    public interface ICookieIdentifier
    {
        public void SetId(IHttpContext context);
        public bool TryGetCurrentClientId(IHttpContext context, out string id);
    }
}
