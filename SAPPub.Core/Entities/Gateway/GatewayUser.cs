using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.Gateway
{
    public class GatewayUser : GatewayMetadata
    {
        public string EmailAddress { get; set; } = string.Empty;
        public string SignUpMagic { get; set; } = string.Empty;
        public Guid LocalAuthorityId { get; set; }
        public bool CookiePrefs { get; set; }
        public bool OptedOutOfComms { get; set; }
        public bool SentSurvey { get; set; }
        public bool ConfirmedSignup { get; set; }
        public DateTime TimerStartedOn { get; set; }
    }
}
