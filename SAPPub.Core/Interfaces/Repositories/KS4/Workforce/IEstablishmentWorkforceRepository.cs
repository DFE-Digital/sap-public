using SAPPub.Core.Entities.KS4.Workforce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Workforce
{
    public interface IEstablishmentWorkforceRepository
    {
        IEnumerable<EstablishmentWorkforce> GetAllEstablishmentWorkforce();
        EstablishmentWorkforce GetEstablishmentWorkforce(string urn);
    }
}
