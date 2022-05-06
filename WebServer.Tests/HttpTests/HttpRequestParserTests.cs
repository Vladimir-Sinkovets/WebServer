using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using WebServer;
using FluentAssertions;

namespace WebServer.Tests.HttpTests
{
    public class HttpRequestParserTests
    {
        [Fact]
        public void Should_ReturnRightHttpMethodName()
        {
            // Arrange
            string httpRequestString = "GET /favicon.ico HTTP/1.1" +
                "Host: 127.0.0.1:8888" +
                "Connection: keep-alive" +
                "sec-ch-ua: \" Not A;Brand\";v=\"99\", \"Chromium\";v=\"100\", \"Google Chrome\";v=\"100\"" +
                "sec-ch-ua-mobile: ?0" +
                "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36" +
                "sec-ch-ua-platform: \"Windows\"" +
                "Accept: image/avif,image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8" +
                "Sec-Fetch-Site: same-origin" +
                "Sec-Fetch-Mode: no-cors" +
                "Sec-Fetch-Dest: image" +
                "Referer: http://127.0.0.1:8888/" +
                "Accept-Encoding: gzip, deflate, br" +
                "Accept-Language: en-US,en;q=0.9,ru;q=0.8" +
                "Cookie: id=a3fWa";

            // Act
            HttpRequest httpRequest = new HttpRequest(httpRequestString);

            // Assert
            httpRequest.Method.Should().Be("GET");
        }
    }
}
