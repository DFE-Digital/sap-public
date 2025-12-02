using SAPPub.Core.Entities.KS4.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance
{
    public interface ILAPerformanceService
    {
        IEnumerable<LAPerformance> GetAllLAPerformance();
        LAPerformance GetLAPerformance(string laCode);
    }
}
