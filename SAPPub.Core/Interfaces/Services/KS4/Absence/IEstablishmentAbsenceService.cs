using SAPPub.Core.Entities.KS4.Absence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services.KS4.Absence
{
    public interface IEstablishmentAbsenceService
    {
        IEnumerable<EstablishmentAbsence> GetAllEstablishmentAbsence();
        EstablishmentAbsence GetEstablishmentAbsence(string urn);
    }
}
