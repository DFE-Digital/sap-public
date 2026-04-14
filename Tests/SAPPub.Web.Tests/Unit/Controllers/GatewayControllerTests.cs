using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Gateway;
using SAPPub.Web.Areas.Gateway.Controllers;
using SAPPub.Web.Areas.Gateway.ViewModels;
using SAPPub.Web.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Web.Tests.Unit.Controllers
{
    public class GatewayControllerTests
    {
        private readonly Mock<IGatewayUserService> _mockUserService = new();
        private readonly Mock<IGatewayLocalAuthorityService> _mockLocalAuthorityService = new();
        private readonly Mock<IGatewayUserAuditService> _mockAuditService = new();
        private readonly Mock<IGatewayUserLAService> _mockGatewayUserLAService = new();
        private readonly Mock<IEmailService> _mockEmailService = new();
        private readonly Mock<ILogger<GatewayController>> _mockLogger = new();

        private GatewayController CreateController(GatewayOptions? options = null, DefaultHttpContext? context = null)
        {
            var opts = Options.Create(options ?? new GatewayOptions { AllowedDays = 2, Enabled = true });
            var controller = new GatewayController(
                _mockUserService.Object,
                _mockLocalAuthorityService.Object,
                _mockAuditService.Object,
                _mockGatewayUserLAService.Object,
                _mockEmailService.Object,
                _mockLogger.Object,
                opts);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context ?? new DefaultHttpContext()
            };

            return controller;
        }

        [Fact]
        public async Task Welcome_Get_InvalidLocalAuthority_ReturnsGatewayErrorView()
        {
            // Arrange
            _mockLocalAuthorityService.Setup(s => s.GetByName(It.IsAny<string>())).ReturnsAsync((GatewayLocalAuthority?)null);
            var controller = CreateController();

            // Act
            var result = await controller.Welcome("invalid") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GatewayError", result.ViewName);
        }

        [Fact]
        public async Task Welcome_Get_ValidLocalAuthority_ReturnsViewModel()
        {
            // Arrange
            var la = new GatewayLocalAuthority { Id = Guid.NewGuid(), LocalAuthorityName = "Test LA" };
            _mockLocalAuthorityService.Setup(s => s.GetByName("test-la")).ReturnsAsync(la);
            var controller = CreateController();

            // Act
            var result = await controller.Welcome("test-la") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<GatewayWelcomeViewModel>(result.Model);
            Assert.Equal(la.LocalAuthorityName, model.LocalAuthority);
            Assert.Equal(la.Id, model.LocalAuthorityId);
            // view name null means default view
            Assert.Null(result.ViewName);
        }

        [Theory]
        [InlineData("new", "NewVisitor")]
        [InlineData("return", "Returning")]
        public void Welcome_Post_Redirects_Based_On_NewOrReturning(string newOrReturning, string expectedAction)
        {
            // Arrange
            var model = new GatewayWelcomeViewModel { NewOrReturning = newOrReturning, LocalAuthority = "la-name" };
            var controller = CreateController();

            // Act
            var result = controller.Welcome(model, "la-name") as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAction, result.ActionName);
            Assert.Equal(model.LocalAuthority, result.RouteValues["id"]);
        }

        [Fact]
        public async Task Returning_Post_UserNotFound_AddsModelErrorAndReturnsView()
        {
            // Arrange
            _mockUserService.Setup(s => s.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((GatewayUser?)null);
            var controller = CreateController();
            var vm = new GatewayReturningViewModel { EmailAddress = "noone@example.com" };

            // Act
            var result = await controller.Returning(vm, "la") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(vm, result.Model);
            Assert.True(controller.ModelState.ContainsKey("EmailAddress"));
        }

        [Fact]
        public async Task Returning_Post_UserExpired_AddsModelErrorAndReturnsView()
        {
            // Arrange
            var allowedDays = 1;
            var options = new GatewayOptions { AllowedDays = allowedDays };
            var http = new DefaultHttpContext();
            var controller = CreateController(options, http);

            var user = new GatewayUser
            {
                Id = Guid.NewGuid(),
                EmailAddress = "expired@example.com",
                TimerStartedOn = DateTime.UtcNow.AddDays(-(allowedDays + 1))
            };

            _mockUserService.Setup(s => s.GetByEmailAsync(user.EmailAddress)).ReturnsAsync(user);

            // Act
            var vm = new GatewayReturningViewModel { EmailAddress = user.EmailAddress };
            var result = await controller.Returning(vm, "la") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(vm, result.Model);
            Assert.True(controller.ModelState.ContainsKey("EmailAddress"));
        }

        [Fact]
        public async Task Returning_Post_ValidUser_SetsCookie_Audits_AndRedirects()
        {
            // Arrange
            var options = new GatewayOptions { AllowedDays = 2 };
            var http = new DefaultHttpContext();
            var controller = CreateController(options, http);

            var user = new GatewayUser
            {
                Id = Guid.NewGuid(),
                EmailAddress = "user@example.com",
                TimerStartedOn = DateTime.UtcNow
            };

            _mockUserService.Setup(s => s.GetByEmailAsync(user.EmailAddress)).ReturnsAsync(user);
            _mockAuditService.Setup(a => a.Insert(user.Id, "Login"));

            // Act
            var vm = new GatewayReturningViewModel { EmailAddress = user.EmailAddress };
            var result = await controller.Returning(vm, "la") as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
            // cookie should be appended to response headers
            var setCookie = http.Response.Headers["Set-Cookie"].ToString();
            Assert.Contains("gateway=", setCookie);
            Assert.Contains(user.Id.ToString(), setCookie);
            _mockAuditService.Verify(a => a.Insert(user.Id, "Login"), Times.Once);
        }

        [Fact]
        public async Task NewVisitor_Get_LA_NotFound_ReturnsGatewayErrorView()
        {
            // Arrange
            _mockLocalAuthorityService.Setup(s => s.GetByName(It.IsAny<string>())).ReturnsAsync((GatewayLocalAuthority?)null);
            var controller = CreateController();

            // Act
            var result = await controller.NewVisitor("missing-la") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GatewayError", result.ViewName);
        }

        [Fact]
        public async Task NewVisitor_Post_UserAlreadyExists_ReturnsViewWithModelError()
        {
            // Arrange
            var existingUser = new GatewayUser { Id = Guid.NewGuid(), EmailAddress = "exists@example.com" };
            _mockUserService.Setup(s => s.GetByEmailAsync(existingUser.EmailAddress)).ReturnsAsync(existingUser);

            var controller = CreateController();
            var vm = new GatewayNewUserViewModel { EmailAddress = existingUser.EmailAddress, LocalAuthorityId = Guid.NewGuid(), LocalAuthorityName = "la" };

            // Act
            var result = await controller.NewVisitor(vm, "la") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(vm, result.Model);
            Assert.True(controller.ModelState.ContainsKey("EmailAddress"));
        }

        [Fact]
        public async Task NewVisitor_Post_SuccessfulRegistration_CreatesUser_Audits_SendsEmail_SetsCookies()
        {
            var newUserId = Guid.NewGuid();

            var vm = new GatewayNewUserViewModel
            {
                EmailAddress = "new@example.com",
                LocalAuthorityId = Guid.NewGuid(),
                LocalAuthorityName = "some-la",
                AcceptCookies = "true",
            };

            var gatewayUser = new GatewayUser()
            {
                Id = newUserId,
                EmailAddress = vm.EmailAddress,
                LocalAuthorityId = vm.LocalAuthorityId,
                SignUpMagic = Guid.NewGuid().ToString().Replace("-", "")
            };

            // Arrange
            _mockUserService.Setup(s => s.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((GatewayUser?)null);            
            
            _mockUserService.Setup(s => s.InsertAsync(It.IsAny<GatewayUser>())).ReturnsAsync(newUserId);
            _mockGatewayUserLAService.Setup(s => s.CanRegisterNewUsers(It.IsAny<Guid>())).ReturnsAsync(true);

            _mockUserService.Setup(s => s.GetById(newUserId)).ReturnsAsync(gatewayUser);

            var http = new DefaultHttpContext();
            var controller = CreateController(null, http);



            // Act
            var result = await controller.NewVisitor(vm, "some-la") as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Complete", result.ActionName);
            var setCookie = http.Response.Headers["Set-Cookie"].ToString();
            // analytics preference cookie present
            Assert.Contains("analytics_preference=", setCookie);
            _mockAuditService.Verify(a => a.Insert(newUserId, "Register"), Times.Once);
            _mockEmailService.Verify(e => e.SendGatewayEmail(gatewayUser.EmailAddress, gatewayUser.Id.ToString(), gatewayUser.SignUpMagic), Times.Once);
        }
    }
}
