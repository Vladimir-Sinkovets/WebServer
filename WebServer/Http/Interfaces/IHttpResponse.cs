using WebServer.Enums;

namespace WebServer.Http.Interfaces
{
    public interface IHttpResponse
    {
        string HttpVersion { get; set; }
        StatusCode StatusCode { get; set; }
        string Content { get; set; }
        string ContentType { get; set; }
        string Connection { get; set; }
        IResponseCookie Cookie { get; }
    }
}