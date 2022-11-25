using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using WebServer.Http;
using WebServer.Http.Interfaces;
using WebServer.Services.Http.Model;
using WebServer.Services.Http.Exceptions;
using WebServer.Services.Http.Enums;

namespace WebServer.Tests.HttpTests
{
    public class HttpRequestTests
    {
        private readonly string _httpRequestString;

        public HttpRequestTests()
        {
            _httpRequestString =
                "GET /favicon.ico?id=10 HTTP/1.1\n" +
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
            HttpRequest httpRequest = new HttpRequest(_httpRequestString, null);

            // Assert
            httpRequest.Method.Should().Be(HttpMethod.GET);
        }
        [Fact]
        public void Should_ReturnHttpPath()
        {
            // Arrange

            // Act
            HttpRequest httpRequest = new HttpRequest(_httpRequestString, null);

            // Assert
            httpRequest.Path.Should().Be("/favicon.ico");
        }
        [Fact]
        public void Should_ReturnHttpCookie()
        {
            // Arrange

            // Act
            HttpRequest httpRequest = new HttpRequest(_httpRequestString, null);

            // Assert
            httpRequest.Cookie.TryGetValue("id", out string id);

            id.Should().Be("a3fWa");
        }
        [Fact]
        public void Should_ReturnHeader()
        {
            // Arrange

            // Act
            HttpRequest httpRequest = new HttpRequest(_httpRequestString, null);

            // Assert
            httpRequest.Headers["sec-fetch-site"].Should().Be("same-origin");
        }
        [Fact]
        public void Should_ReturnQueryString()
        {
            // Arrange

            // Act
            HttpRequest httpRequest = new HttpRequest(_httpRequestString, null);

            // Assert
            httpRequest.QueryString.Should().Be("id=10");
        }
        [Fact]
        public void Should_ReturnQuery()
        {
            // Arrange

            // Act
            HttpRequest httpRequest = new HttpRequest(_httpRequestString, null);

            // Assert
            httpRequest.Query["id"].Should().Be("10");
        }
        [Fact]
        public void Should_ReturnContentType()
        {
            // Arrange
            string httpRequestString = 
                "POST /test/test.php HTTP/1.1\n" +
                "Host: test.com\n" +
                "Content-Type: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            HttpRequest httpRequest = new HttpRequest(httpRequestString, null);

            // Assert
            httpRequest.ContentType.Should().Be("text");
        }


        [Fact]
        public void Should_ThrowHttpParseException_WhenMethodIsWrong()
        {
            // Arrange
            string httpRequestString =
                "qweqqweqqw /test/test.php HTTP/1.1\n" +
                "Host: test.com\n" +
                "Content-Type: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            Action act = () => new HttpRequest(httpRequestString, null);

            // Assert
            act.Should().Throw<HttpParseException>();
        }
        [Fact]
        public void Should_ThrowHttpParseException_WhenMethodIsEmpty()
        {
            // Arrange
            string httpRequestString =
                "/test/test.php HTTP/1.1\n" +
                "Host: test.com\n" +
                "Content-Type: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            Action act = () => new HttpRequest(httpRequestString, null);

            // Assert
            act.Should().Throw<HttpParseException>();
        }
        [Fact]
        public void Should_ThrowHttpParseException_WhenPathIsEmpty()
        {
            // Arrange
            string httpRequestString =
                "GET  HTTP/1.1\n" +
                "Host: test.com\n" +
                "Content-Type: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            Action act = () => new HttpRequest(httpRequestString, null);

            // Assert
            act.Should().Throw<HttpParseException>();
        }
        [Fact]
        public void Should_ThrowHttpParseException_WhenCookieIsWrong()
        {
            // Arrange
            string httpRequestString =
                "POST test.php HTTP/1.1\n" +
                "Host: test.com\n" +
                "Content-Type: text\n" +
                "Cookie: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            Action act = () => new HttpRequest(httpRequestString, null);

            // Assert
            act.Should().Throw<HttpParseException>();
        }
        [Fact]
        public void Should_ThrowHttpParseException_WhenHeaderIsWrong()
        {
            // Arrange
            string httpRequestString =
                "POST test.php HTTP/1.1\n" +
                "Host -> test.com\n" +
                "Content-Type: text\n" +
                "Cookie: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            Action act = () => new HttpRequest(httpRequestString, null);

            // Assert
            act.Should().Throw<HttpParseException>();
        }
        [Fact]
        public void Should_ThrowHttpParseException_WhenHeaderIsEmpty()
        {
            // Arrange
            string httpRequestString =
                "POST test.php HTTP/1.1\n" +
                "Host : \n" +
                "Content-Type: text\n" +
                "Cookie: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            Action act = () => new HttpRequest(httpRequestString, null);

            // Assert
            act.Should().Throw<HttpParseException>();
        }
        [Fact]
        public void Should_ThrowHttpParseException_WhenUrlContainSameQueryParameters()
        {
            // Arrange
            string httpRequestString =
                "POST test.php?test1=test&test1=test2 HTTP/1.1\n" +
                "Host -> test.com\n" +
                "Content-Type: text\n" +
                "Cookie: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            Action act = () => new HttpRequest(httpRequestString, null);

            // Assert
            act.Should().Throw<HttpParseException>();
        }
        [Fact]
        public void Should_ThrowHttpParseException_WhenQueryParameterHasNotValue()
        {
            // Arrange
            string httpRequestString =
                "POST test.php?test1=test&test2 HTTP/1.1\n" +
                "Host -> test.com\n" +
                "Content-Type: text\n" +
                "Cookie: text\n" +
                "Connection : Keep-Alive\n" +
                "\n" +
                "name1 = value1 & name2 = value2";
            // Act
            Action act = () => new HttpRequest(httpRequestString, null);

            // Assert
            act.Should().Throw<HttpParseException>();
        }
    }
}