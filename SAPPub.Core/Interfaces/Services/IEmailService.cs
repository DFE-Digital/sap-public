using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services
{
    public interface IEmailService
    {
        void SendGatewayEmail(string emailAddress);
    }
}
