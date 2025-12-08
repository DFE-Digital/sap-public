using SAPPub.Core.Entities.KS4.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance
{
    public interface IEstablishmentPerformanceService
    {
        IEnumerable<EstablishmentPerformance> GetAllEstablishmentPerformance();
        EstablishmentPerformance GetEstablishmentPerformance(string urn);
    }
}
