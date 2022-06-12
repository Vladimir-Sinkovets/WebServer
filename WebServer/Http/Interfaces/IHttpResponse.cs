using WebServer.Enums;
using System.Collections.Generic;

namespace WebServer.Http.Interfaces
{
    public interface IHttpResponse
    {
        byte[] Body { get; set; }
        string Connection { get; set; }
        string HttpVersion { get; set; }
        string ContentType { get; set; }
        StatusCode StatusCode { get; set; }
        IDictionary<string, string> Headers { get; set; }
        IResponseCookieCollection Cookie { get; }
    }
}