namespace PassKitHelper
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class PassKitMiddlewareTests_DevicesUnreg : PassKitMiddlewareTestBase
    {
        private readonly PathString pathBase = "/v1/devices";

        private readonly string deviceLibId = "somedevice";

        private readonly string passType = "sometype";

        private readonly string passSerial = "someserial";

        private readonly string authToken = "sometoken";

        public PassKitMiddlewareTests_DevicesUnreg()
        {
            httpContext.Request.Method = "DELETE";
            httpContext.Request.Path = pathBase + $"/{deviceLibId}/registrations/{passType}/{passSerial}";
            httpContext.Request.Headers["Authorization"] = $"ApplePass {authToken}";
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
            passkitServiceMock
                .Setup(x => x.UnregisterDeviceAsync(deviceLibId, passType, passSerial, authToken))
                .ReturnsAsync(200)
                .Verifiable();

            await middleware.InvokeAsync(httpContext);

            Assert.Equal(200, httpContext.Response.StatusCode);
            mocks.Verify();
        }
    }
}
