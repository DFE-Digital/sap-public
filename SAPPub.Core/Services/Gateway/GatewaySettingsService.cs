using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Repositories.Gateway;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services.Gateway
{
    public class GatewaySettingsService : IGatewaySettingsService
    {
        private readonly IGenericCRUDRepository<GatewaySettings> _settingsRepository;
        private readonly ILogger<GatewaySettingsService> _logger;

        public GatewaySettingsService(IGenericCRUDRepository<GatewaySettings> settingsRepository, ILogger<GatewaySettingsService> logger)
        {
            _settingsRepository = settingsRepository;
            _logger = logger;
        }
        public bool IsServiceLive()
        {
            var settingValue = _settingsRepository.ReadAll()?.FirstOrDefault(x => x.SettingName == "GlobalEnable");
            if (settingValue == null)
            {
                _logger.LogWarning("GlobalEnable setting not found. Defaulting to false.");
                return false;
            }

            if (bool.TryParse(settingValue.SettingValue, out bool isLive))
            {
                return isLive;
            }
            else
            {
                _logger.LogWarning("GlobalEnable setting value '{SettingValue}' is not a valid boolean. Defaulting to false.", settingValue.SettingValue);
                return false;
            }
        }
    }
}
