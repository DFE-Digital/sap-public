using SAPPub.Core.Entities.KS4.Absence;

namespace SAPPub.Core.Interfaces.Services.KS4.Absence
{
    public interface ILAAbsenceService
    {
        IEnumerable<LAAbsence> GetAllLAAbsence();
        LAAbsence GetLAAbsence(string urn);
    }
}
