using SAPPub.Core.ServiceModels.Overview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services.Overview
{
    public interface IOverviewService
    {
        Task<OverviewModel?> GetOverviewAsync(string urn,  CancellationToken ct = default);
    }
}
