using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.KS4.Workforce;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class Establishment
    {
        public string URN { get; set; } = string.Empty;
        public string LAId { get; set; } = string.Empty;

        public string EstablishmentName { get; set; } = string.Empty;
        public int? EstablishmentNumber { get; set; }

        public int? TrustsId { get; set; }
        public string TrustName { get; set; } = string.Empty;

        public int? AdmissionsPolicyId { get; set; }
        public string AdmissionPolicy { get; set; } = string.Empty;
        public string DistrictAdministrativeId { get; set; } = string.Empty;
        public string DistrictAdministrativeName { get; set; } = string.Empty;
        public int? PhaseOfEducationId { get; set; }
        public string PhaseOfEducationName { get; set; } = string.Empty;
        public int? GenderId { get; set; }
        public string GenderName { get; set; } = string.Empty;
        public int? OfficialSixthFormId { get; set; }
        public string LANAme { get; set; } = string.Empty;
        public int? ReligiousCharacterId { get; set; }
        public string ReligiousCharacterName { get; set; } = string.Empty;
        public string TelephoneNum { get; set; } = string.Empty;
        public int TotalPupils { get; set; }
        public int? TypeOfEstablishmentId { get; set; }
        public string TypeOfEstablishmentName { get; set; } = string.Empty;
        public int? ResourcedProvision { get; set; }
        public string ResourcedProvisionName { get; set; } = string.Empty;
        public int UKPRN { get; set; }
        public string UrbanRuralId { get; set; } = string.Empty;
        public string UrbanRuralName { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public int? Easting { get; set; }
        public int? Northing { get; set; }

        public EstablishmentPerformance KS4Performance { get; set; } = new();
        public LAPerformance LAPerformance { get; set; } = new();
        public EnglandPerformance EnglandPerformance { get; set; } = new();

        public EstablishmentDestinations EstablishmentDestinations { get; set; } = new();
        public LADestinations LADestinations { get; set; } = new();
        public EnglandDestinations EnglandDestinations { get; set; } = new();

        public EstablishmentAbsence Absence { get; set; } = new(); // Will eventually need one per phase
        public EstablishmentWorkforce Workforce { get; set; } = new(); // Will eventually need one per phase



    }
}
