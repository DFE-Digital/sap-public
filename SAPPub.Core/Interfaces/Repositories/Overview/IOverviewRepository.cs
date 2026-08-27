using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPPub.Core.Entities.Overview;

namespace SAPPub.Core.Interfaces.Repositories.Overview
{
    public interface IOverviewRepository
    {
        Task<Entities.Overview.Overview?> GetOverviewAsync(string urn, CancellationToken ct = default);
    }
}
