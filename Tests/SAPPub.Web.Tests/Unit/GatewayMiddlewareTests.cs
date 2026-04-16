using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Services.Gateway;
using SAPPub.Web.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Web.Tests.Unit
{
    public class GatewayMiddlewareTests
    {
        private RequestDelegate Next(Action onInvoke) => _ =>
        {
            onInvoke();
            return Task.CompletedTask;
        };

        [Fact]
        public async Task InvokeAsync_UnrestrictedUrl_CallsNext()
        {
            // Arrange
            var invoked = false;
            var http = new DefaultHttpContext();
            http.Request.Path = "/health";
            var mockLogger = new Mock<ILogger<GatewayMiddleware>>();
            var mockUserService = new Mock<IGatewayUserService>();
            var mockSettings = new Mock<IGatewaySettingsService>();
            var middleware = new GatewayMiddleware(Next(() => invoked = true), mockLogger.Object, mockUserService.Object, mockSettings.Object);

            // Act
            await middleware.InvokeAsync(http);

            // Assert
            Assert.True(invoked);
            Assert.NotEqual(302, http.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ServiceNotLive_RedirectsToClosed()
        {
            // Arrange
            var invoked = false;
            var http = new DefaultHttpContext();
            http.Request.Path = "/any";
            var mockLogger = new Mock<ILogger<GatewayMiddleware>>();
            var mockUserService = new Mock<IGatewayUserService>();
            var mockSettings = new Mock<IGatewaySettingsService>();
            mockSettings.Setup(s => s.IsServiceLive()).ReturnsAsync(false);

            var middleware = new GatewayMiddleware(Next(() => invoked = true), mockLogger.Object, mockUserService.Object, mockSettings.Object);

            // Act
            await middleware.InvokeAsync(http);

            // Assert
            Assert.False(invoked);
            Assert.Equal(302, http.Response.StatusCode);
            Assert.Equal("/gateway/closed", http.Response.Headers["Location"].ToString());
        }

        [Fact]
        public async Task InvokeAsync_GatewayPath_WithValidNonExpiredUser_RedirectsToHome()
        {
            // Arrange
            var invoked = false;
            var http = new DefaultHttpContext();
            http.Request.Path = "/gateway/whatever";
            var userId = Guid.NewGuid();
            http.Request.Headers["Cookie"] = $"gateway={userId}";

            var mockLogger = new Mock<ILogger<GatewayMiddleware>>();
            var mockUserService = new Mock<IGatewayUserService>();
            var mockSettings = new Mock<IGatewaySettingsService>();
            mockSettings.Setup(s => s.IsServiceLive()).ReturnsAsync(true);

            var user = new GatewayUser { Id = userId };
            mockUserService.Setup(s => s.GetById(userId)).ReturnsAsync(user);
            mockUserService.Setup(s => s.IsUserExpiredAsync(userId)).ReturnsAsync(false);

            var middleware = new GatewayMiddleware(Next(() => invoked = true), mockLogger.Object, mockUserService.Object, mockSettings.Object);

            // Act
            await middleware.InvokeAsync(http);

            // Assert
            Assert.False(invoked);
            Assert.Equal(302, http.Response.StatusCode);
            Assert.Equal("/", http.Response.Headers["Location"].ToString());
        }

        [Fact]
        public async Task InvokeAsync_GatewayPath_WithExpiredUser_RedirectsToSessionEnded()
        {
            // Arrange
            var invoked = false;
            var http = new DefaultHttpContext();
            http.Request.Path = "/gateway/some";
            var userId = Guid.NewGuid();
            http.Request.Headers["Cookie"] = $"gateway={userId}";

            var mockLogger = new Mock<ILogger<GatewayMiddleware>>();
            var mockUserService = new Mock<IGatewayUserService>();
            var mockSettings = new Mock<IGatewaySettingsService>();
            mockSettings.Setup(s => s.IsServiceLive()).ReturnsAsync(true);

            var user = new GatewayUser { Id = userId };
            mockUserService.Setup(s => s.GetById(userId)).ReturnsAsync(user);
            mockUserService.Setup(s => s.IsUserExpiredAsync(userId)).ReturnsAsync(true);

            var middleware = new GatewayMiddleware(Next(() => invoked = true), mockLogger.Object, mockUserService.Object, mockSettings.Object);

            // Act
            await middleware.InvokeAsync(http);

            // Assert
            Assert.False(invoked);
            Assert.Equal(302, http.Response.StatusCode);
            Assert.Equal("/Gateway/SessionEnded", http.Response.Headers["Location"].ToString());
        }

        [Fact]
        public async Task InvokeAsync_RestrictedUrl_NoCookie_RedirectsToError()
        {
            // Arrange
            var invoked = false;
            var http = new DefaultHttpContext();
            http.Request.Path = "/school";
            var mockLogger = new Mock<ILogger<GatewayMiddleware>>();
            var mockUserService = new Mock<IGatewayUserService>();
            var mockSettings = new Mock<IGatewaySettingsService>();
            mockSettings.Setup(s => s.IsServiceLive()).ReturnsAsync(true);

            var middleware = new GatewayMiddleware(Next(() => invoked = true), mockLogger.Object, mockUserService.Object, mockSettings.Object);

            // Act
            await middleware.InvokeAsync(http);

            // Assert
            Assert.False(invoked);
            Assert.Equal(302, http.Response.StatusCode);
            Assert.Equal("/Gateway/Error", http.Response.Headers["Location"].ToString());
        }

        [Fact]
        public async Task InvokeAsync_RestrictedUrl_WithValidUser_CallsNext()
        {
            // Arrange
            var invoked = false;
            var http = new DefaultHttpContext();
            http.Request.Path = "/school/details";
            var userId = Guid.NewGuid();
            http.Request.Headers["Cookie"] = $"gateway={userId}";

            var mockLogger = new Mock<ILogger<GatewayMiddleware>>();
            var mockUserService = new Mock<IGatewayUserService>();
            var mockSettings = new Mock<IGatewaySettingsService>();
            mockSettings.Setup(s => s.IsServiceLive()).ReturnsAsync(true);

            var user = new GatewayUser { Id = userId };
            mockUserService.Setup(s => s.GetById(userId)).ReturnsAsync(user);
            mockUserService.Setup(s => s.IsUserExpiredAsync(userId)).ReturnsAsync(false);

            var middleware = new GatewayMiddleware(Next(() => invoked = true), mockLogger.Object, mockUserService.Object, mockSettings.Object);

            // Act
            await middleware.InvokeAsync(http);

            // Assert
            Assert.True(invoked);
            Assert.NotEqual(302, http.Response.StatusCode);
        }
    }
}
