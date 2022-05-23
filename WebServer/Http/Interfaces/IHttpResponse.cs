namespace WebServer.Http.Interfaces
{
    public interface IHttpResponse
    {
        public string HttpVersion { get; set; }
        public string StatusCode { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public string Connection { get; set; }
        public IResponseCookie Cookie { get; }
    }
}