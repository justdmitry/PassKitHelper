namespace PassKitHelper
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class PassKitMiddlewareTests_DevicesReg : PassKitMiddlewareTestBase
    {
        private readonly PathString pathBase = "/v1/devices";

        private readonly string deviceLibId = "somedevice";

        private readonly string passType = "sometype";

        private readonly string passSerial = "someserial";

        private readonly string authToken = "sometoken";

        private readonly MemoryStream body;

        public PassKitMiddlewareTests_DevicesReg()
        {
            httpContext.Request.Method = "POST";
            httpContext.Request.Path = pathBase + $"/{deviceLibId}/registrations/{passType}/{passSerial}";
            httpContext.Request.Headers["Authorization"] = $"ApplePass {authToken}";

            body = new MemoryStream();
            httpContext.Request.Body = body;
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
            httpContext.Request.Path = pathBase + $"/{deviceLibId}/registrations/{passType}";

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(400, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task DoNothingWithoutAuth()
        {
            httpContext.Request.Headers.Remove("Authorization");

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(401, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task DoNothingWithWrongAuth()
        {
            httpContext.Request.Headers["Authorization"] = "invalid value";

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(401, httpContext.Response.StatusCode);
            mocks.Verify();
        }

        [Fact]
        public async Task ServiceIsCalled()
        {
            using (var sw = new StreamWriter(body, Encoding.Default, 2048, true))
            {
                await sw.WriteAsync("{ \"pushToken\": \"foo bar\" }");
            }

            body.Position = 0;

            passkitServiceMock
                .Setup(x => x.RegisterDeviceAsync(deviceLibId, passType, passSerial, authToken, "foo bar"))
                .ReturnsAsync(200)
                .Verifiable();

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
            mocks.Verify();
        }
    }
}
