﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer.Services.HttpContextFactories
{
    public interface IHttpContextFactory
    {
        HttpContext CreateInstance(byte[] data, IServiceProvider provider);
    }
}
