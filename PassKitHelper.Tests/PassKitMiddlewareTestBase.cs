namespace PassKitHelper
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class PassKitMiddlewareTestBase
    {
        protected readonly MockRepository mocks;

        protected readonly Mock<IServiceProvider> serviceProviderMock;

        protected readonly Mock<IPassKitService> passkitServiceMock;

        protected readonly HttpContext httpContext;

        protected readonly PassKitMiddleware middleware;

        protected readonly RequestDelegate emptyNext = context => Task.CompletedTask;

        public PassKitMiddlewareTestBase()
        {
            mocks = new MockRepository(MockBehavior.Strict);

            httpContext = new DefaultHttpContext();

            serviceProviderMock = mocks.Create<IServiceProvider>();
            passkitServiceMock = mocks.Create<IPassKitService>();

            serviceProviderMock.Setup(x => x.GetService(typeof(IPassKitService))).Returns(passkitServiceMock.Object);

            httpContext.RequestServices = serviceProviderMock.Object;

            var loggerMock = mocks.Create<ILogger<PassKitMiddleware>>(MockBehavior.Loose);

            middleware = new PassKitMiddleware(emptyNext, loggerMock.Object);
        }
    }
}
