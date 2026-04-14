using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class EmailServiceTests
    {
        private readonly Mock<IEmailRepository> _mockRepo;
        private readonly Mock<ILogger<EmailService>> _mockLogger;
        private readonly EmailService _service;

        public EmailServiceTests()
        {
            _mockRepo = new Mock<IEmailRepository>();
            _mockLogger = new Mock<ILogger<EmailService>>();
            _service = new EmailService(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public void SendEmail_AllLegit()
        {
            // Arrange
            var email = "johndoe@email.com";
            var userId = "4e2ab58d-578d-4299-bc2e-dd92fd8e3e86";
            var validation = "4dadf6923a14485181e91c81874671e6";

            // Act
            try
            {
                _service.SendGatewayEmail(email, userId, validation);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Fail("Exception thrown, but shouldn't " + ex.Message);
            }
        }

        [Theory]
        [InlineData("johndoe@email.com", "", "")]
        [InlineData("", "4e2ab58d-578d-4299-bc2e-dd92fd8e3e86", "")]
        [InlineData("", "", "4e2ab58d-578d-4299-bc2e-dd92fd8e3e86")]
        public void SendEmail_WithError(string email, string userId, string validation)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _service.SendGatewayEmail(email, userId, validation));

            // Assert
            Assert.Contains("Error sending email, missing argument", ex.Message);
        }

        [Fact]
        public void SendEmail_WithError_NullEmail()
        {
            // Arrange

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var ex = Assert.Throws<ArgumentException>(() => _service.SendGatewayEmail(null, "", ""));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Assert
            Assert.Contains("Error sending email, missing argument", ex.Message);
        }

        [Fact]
        public void SendEmail_WithError_NullUserId()
        {
            // Arrange

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var ex = Assert.Throws<ArgumentException>(() => _service.SendGatewayEmail("",null, ""));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Assert
            Assert.Contains("Error sending email, missing argument", ex.Message);
        }
        [Fact]
        public void SendEmail_WithError_NullValidation()
        {
            // Arrange

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var ex = Assert.Throws<ArgumentException>(() => _service.SendGatewayEmail("", "", null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Assert
            Assert.Contains("Error sending email, missing argument", ex.Message);
        }
    }
}
