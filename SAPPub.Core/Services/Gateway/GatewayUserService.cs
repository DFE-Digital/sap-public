using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Gateway;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Gateway;
using SAPPub.Web.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.Gateway
{
    public class GatewayUserService : IGatewayUserService
    {
        private readonly IGatewayUserRepository _gatewayUserRepository;
        private readonly ILogger<GatewayUserService> _logger;
        private readonly IOptions<GatewayOptions> _options;

        public GatewayUserService(IGatewayUserRepository gatewayUserRepository, ILogger<GatewayUserService> logger, IOptions<GatewayOptions> options)
        {
            _gatewayUserRepository = gatewayUserRepository;
            _logger = logger;
            _options = options;
        }

        public async Task<GatewayUser?> GetByEmailAsync(string email)
        {
            var allUsers = await _gatewayUserRepository.GetAllAsync();
            return allUsers.FirstOrDefault(u => u.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<GatewayUser?> GetById(Guid id)
        {
            return await _gatewayUserRepository.GetByIdAsync(id);
        }

        public async Task<bool> IsUserExpiredAsync(Guid id)
        {
            var user = await _gatewayUserRepository.GetByIdAsync(id);
            if (user?.Id == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            var userRegistration = user.TimerStartedOn;

            var expiryDate = userRegistration.AddDays(_options.Value.AllowedDays);
            if (expiryDate > DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        public async Task<Guid> InsertAsync(GatewayUser user)
        {
            if (string.IsNullOrWhiteSpace(user.EmailAddress))
            {
                throw new ArgumentException("Email address is required.", nameof(user.EmailAddress));
            }

            user.Id = Guid.NewGuid();
            user.TimerStartedOn = DateTime.UtcNow;
            user.CreatedOn = DateTime.UtcNow;
            user.ModifiedOn = DateTime.UtcNow;
            user.IsDeleted = false;
            user.OptedOutOfComms = false;
            user.SentSurvey = false;
            user.SignUpMagic = $"{Guid.NewGuid().ToString().Replace("-", "")}";
            user.ConfirmedSignup = false;

            return await _gatewayUserRepository.InsertAsync(user) ? user.Id : throw new Exception("Error on Add user");

        }

        public async void UserConfirmed(Guid id)
        {
            var user = await _gatewayUserRepository.GetByIdAsync(id);
            if (user?.Id == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            user.ConfirmedSignup = true;
            var updateStatus = await _gatewayUserRepository.UpdateAsync(user);
            if (!updateStatus)
            {
                throw new Exception($"Couldn't update {id} with confirmed status.");
            }
        }

        public async Task<IEnumerable<GatewayUser>> GetAllAsync()
        {
            return await _gatewayUserRepository.GetAllAsync();
        }
    }
}
