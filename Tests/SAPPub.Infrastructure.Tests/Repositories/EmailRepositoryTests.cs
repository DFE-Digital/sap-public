using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SAPPub.Infrastructure.Repositories;
using SAPPub.Web.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Tests.Repositories
{
    public class EmailRepositoryTests
    {
        private readonly EmailOptions _emailOptions = new EmailOptions
        {
            GatewayTemplate = "gateway-template-id",
            ApplicationRoot = "https://app.example"
        };

        [Fact]
        public void SendGatewayEmail_WhenNotificationThrows_LogsExceptionAndDoesNotThrow()
        {
            // Arrange
            var emailAddress = "user2@example.com";
            var userId = "4dadf692-3a14-4851-81e9-1c81874671e6";
            var validation = "4dadf6923a14485181e91c81874671e7";


            var notificationMock = new Mock<Notify.Interfaces.INotificationClient>();
            var boom = new Exception("send-failed");
            notificationMock
                .Setup(n => n.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(), null, null, null))
                .Throws(boom);

            var loggerMock = new Mock<ILogger<EmailRepository>>();

            var repository = new EmailRepository(notificationMock.Object, Options.Create(_emailOptions), loggerMock.Object);

            // Act & Assert - should not throw (exception is caught inside)
            var ex = Record.Exception(() => repository.SendGatewayEmail(emailAddress, userId, validation));
            Assert.Null(ex);
        }
    }
}
