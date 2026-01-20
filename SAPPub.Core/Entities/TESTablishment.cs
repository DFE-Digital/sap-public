using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class TESTablishment
    {
        public string URN { get; set; } = string.Empty;
        
        public string EstablishmentName { get; set; } = string.Empty;
        
        public string EstablishmentNameClean => TextHelpers.CleanForUrl(EstablishmentName);
        
        public string TrustsId { get; set; } = string.Empty;
        
        public string TrustName { get; set; } = string.Empty;
        
        public string Address => TextHelpers.ConcatListToString([AddressStreet, AddressLocality, AddressAddress3, AddressTown, AddressPostcode]);
        
        public string AddressStreet { get; set; } = string.Empty;
        
        public string AddressLocality { get; set; } = string.Empty;
        
        public string AddressAddress3 { get; set; } = string.Empty;
        
        public string AddressTown { get; set; } = string.Empty;
        
        public string AddressPostcode { get; set; } = string.Empty;
        
        public string AdmissionsPolicyId { get; set; } = string.Empty;

        public string AdmissionPolicy { get; set; } = string.Empty;

        public string AgeRange => TextHelpers.ConcatListToString([AgeRangeLow, AgeRangeHigh], " to ");

        public string DFENumber { get; set; } = string.Empty;

        public string DistrictAdministrativeId { get; set; } = string.Empty;

        public string DistrictAdministrativeName { get; set; } = string.Empty;

        public string PhaseOfEducationId { get; set; } = string.Empty;

        public string PhaseOfEducationName { get; set; } = string.Empty;

        public string GenderId { get; set; } = string.Empty;

        public string GenderName { get; set; } = string.Empty;

        public string Headteacher => TextHelpers.ConcatListToString([HeadteacherTitle, HeadteacherFirstName, HeadteacherLastName], " ");

        public string HeadteacherTitle { get; set; } = string.Empty;

        public string HeadteacherFirstName { get; set; } = string.Empty;

        public string HeadteacherLastName { get; set; } = string.Empty;

        public string HeadteacherPreferredJobTitle { get; set; } = string.Empty;

        public string AgeRangeLow { get; set; } = string.Empty;

        public string AgeRangeHigh { get; set; } = string.Empty;

        public string OfficialSixthFormId { get; set; } = string.Empty;

        public string LAId { get; set; } = string.Empty;

        public string LAName { get; set; } = string.Empty;

        public string ReligiousCharacterId { get; set; } = string.Empty;

        public string ReligiousCharacterName { get; set; } = string.Empty;

        public string TelephoneNum { get; set; } = string.Empty;

        public string TotalPupils { get; set; } = string.Empty;

        public string TypeOfEstablishmentId { get; set; } = string.Empty;

        public string TypeOfEstablishmentName { get; set; } = string.Empty;

        public string ResourcedProvision { get; set; } = string.Empty;

        public string UKPRN { get; set; } = string.Empty;

        public string UrbanRuralId { get; set; } = string.Empty;
        public string UrbanRuralName { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        public string Easting { get; set; } = string.Empty;

        public string Northing { get; set; } = string.Empty;

        public string EstablishmentNumber { get; set; } = string.Empty;

        public EstablishmentPerformance KS4Performance { get; set; } = new();
        
        public LAPerformance LAPerformance { get; set; } = new();
        
        public EnglandPerformance EnglandPerformance { get; set; } = new();

        public EstablishmentDestinations EstablishmentDestinations { get; set; } = new();
        
        public LADestinations LADestinations { get; set; } = new();

        public EnglandDestinations EnglandDestinations { get; set; } = new();

        public EstablishmentAbsence Absence { get; set; } = new(); // Will eventually need one per phase

        public LAAbsence LAAbsence { get; set; } = new();

        public EnglandAbsence EnglandAbsence { get; set; } = new();

        public EstablishmentWorkforce Workforce { get; set; } = new(); // Will eventually need one per phase
    }
}
