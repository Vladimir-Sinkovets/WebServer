using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using WebServer.Http;
using WebServer.Enums;
using WebServer.Http.Interfaces;

namespace WebServer.Tests.HttpTests
{
    public class HttpRequestTests
    {
        private readonly string _httpRequestString;

        public HttpRequestTests()
        {
            _httpRequestString = 
                "GET /favicon.ico HTTP/1.1\n" +
                "Host: 127.0.0.1:8888\n" +
                "Connection: keep-alive\n" +
                "sec-ch-ua: \" Not A;Brand\";v=\"99\", \"Chromium\";v=\"100\", \"Google Chrome\";v=\"100\"\n" +
                "sec-ch-ua-mobile: ?0\n" +
                "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36\n" +
                "sec-ch-ua-platform: \"Windows\"\n" +
                "Accept: image/avif,image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8\n" +
                "Sec-Fetch-Site: same-origin\n" +
                "Sec-Fetch-Mode: no-cors\n" +
                "Sec-Fetch-Dest: image\n" +
                "Referer: http://127.0.0.1:8888/\n" +
                "Accept-Encoding: gzip, deflate, br\n" +
                "Accept-Language: en-US,en;q=0.9,ru;q=0.8\n" +
                "Cookie: id=a3fWa\n\n";
        }
        [Fact]
        public void Should_ReturnHttpMethodName()
        {
            // Arrange

            // Act
            IHttpRequest httpRequest = new HttpRequest(_httpRequestString);

            // Assert
            httpRequest.Method.Should().Be(HttpMethod.GET);
        }
        [Fact]
        public void Should_ReturnHttpPath()
        {
            // Arrange

            // Act
            IHttpRequest httpRequest = new HttpRequest(_httpRequestString);

            // Assert
            httpRequest.Path.Should().Be("/favicon.ico");
        }
        [Fact]
        public void Should_ReturnHttpCookie()
        {
            // Arrange

            // Act
            IHttpRequest httpRequest = new HttpRequest(_httpRequestString);

            // Assert
            httpRequest.Cookie.TryGetValue("id", out string id);
            
            id.Should().Be("a3fWa");
        }
        [Fact]
        public void Should_ReturnHeader()
        {
            // Arrange

            // Act
            IHttpRequest httpRequest = new HttpRequest(_httpRequestString);

            // Assert
            httpRequest.GetHeaderValue("Sec-Fetch-Site").Should().Be("same-origin");
        }
    }
}