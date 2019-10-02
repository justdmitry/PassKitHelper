namespace PassKitHelper
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class PassKitMiddlewareTests_DevicesGet : PassKitMiddlewareTestBase
    {
        private readonly PathString pathBase = "/v1/devices";

        private readonly string deviceLibId = "somedevice";

        private readonly string passType = "sometype";

        private readonly string tag = "sometag";

        private readonly MemoryStream body;

        public PassKitMiddlewareTests_DevicesGet()
        {
            httpContext.Request.Method = "GET";
            httpContext.Request.Path = pathBase + $"/{deviceLibId}/registrations/{passType}";
            httpContext.Request.QueryString = new QueryString($"?passesUpdatedSince={tag}");

            body = new MemoryStream();
            httpContext.Response.Body = body;
        }

        [Fact]
        public async Task DoNothingOnMoreSegments()
        {
            httpContext.Request.Path += "/extra-segment";

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(400, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task DoNothingOnLessSegments()
        {
            httpContext.Request.Path = pathBase + $"/{deviceLibId}/registrations";

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(400, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task ServiceIsCalled()
        {
            passkitServiceMock
                .Setup(x => x.GetAssociatedPassesAsync(deviceLibId, passType, tag))
                .ReturnsAsync((204, null, null))
                .Verifiable();

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(204, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task ServiceIsCalledWithoutTag()
        {
            httpContext.Request.QueryString = QueryString.Empty;

            passkitServiceMock
                .Setup(x => x.GetAssociatedPassesAsync(deviceLibId, passType, null))
                .ReturnsAsync((204, null, null))
                .Verifiable();

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(204, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task ResponseIsWritten()
        {
            var newTag = "newtag";

            var serial1 = "serial1";
            var serial2 = "serial2";

            passkitServiceMock
                .Setup(x => x.GetAssociatedPassesAsync(deviceLibId, passType, tag))
                .ReturnsAsync((200, new[] { serial1, serial2 }, newTag))
                .Verifiable();

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
            mocks.Verify();

            httpContext.Response.Body.Position = 0;
            using var sr = new StreamReader(httpContext.Response.Body);
            var resp = await sr.ReadToEndAsync();

            Assert.Equal("{\"lastUpdated\":\"newtag\",\"serialNumbers\":[\"serial1\",\"serial2\"]}", resp);
        }
    }
}
