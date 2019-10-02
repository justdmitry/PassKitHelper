namespace PassKitHelper
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class PassKitMiddlewareTests_Logs : PassKitMiddlewareTestBase
    {
        private readonly PathString path = "/v1/log";

        private readonly MemoryStream body;

        public PassKitMiddlewareTests_Logs()
        {
            body = new MemoryStream();

            httpContext.Request.Body = body;
            httpContext.Request.Method = "POST";
            httpContext.Request.Path = path;
        }

        [Fact]
        public async Task DoNothingOnEmptyRequest()
        {
            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task DoNothingOnNonPOST()
        {
            httpContext.Request.Method = "GET";

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(405, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task ServiceIsCalled()
        {
            using (var sw = new StreamWriter(body, Encoding.Default, 2048, true))
            {
                await sw.WriteAsync("{ logs: [\"log line 1\", \"log line 2\" ] }");
            }

            body.Position = 0;

            string[]? logs = null;

            passkitServiceMock
                .Setup(x => x.ProcessLogsAsync(It.IsAny<string[]>()))
                .Returns(Task.CompletedTask)
                .Callback<string[]>(value => logs = value)
                .Verifiable();

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
            Assert.Equal(new[] { "log line 1", "log line 2" }, logs);
            mocks.Verify();
        }
    }
}
