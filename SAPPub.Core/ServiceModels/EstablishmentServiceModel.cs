using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Helpers;

namespace SAPPub.Core.ServiceModels;


public class EstablishmentServiceModel
{
    public string URN { get; set; } = string.Empty;

    public string EstablishmentName { get; set; } = string.Empty;

    public string EstablishmentNameClean => TextHelpers.CleanForUrl(EstablishmentName);

    public string? TrustsId { get; set; }

    public string? TrustName { get; set; }

    public string Address => TextHelpers.ConcatListToString([AddressStreet, AddressLocality, AddressAddress3, AddressTown, AddressPostcode]);

    public string AddressStreet { get; set; } = string.Empty;

    public string AddressLocality { get; set; } = string.Empty;

    public string AddressAddress3 { get; set; } = string.Empty;

    public string AddressTown { get; set; } = string.Empty;
    public string AddressCounty { get; set; } = string.Empty;

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

    public string? GSSLACode { get; set; }

    public string ReligiousCharacterId { get; set; } = string.Empty;

    public string ReligiousCharacterName { get; set; } = string.Empty;

    public string TelephoneNum { get; set; } = string.Empty;

    public string TotalPupils { get; set; } = string.Empty;

    public string TypeOfEstablishmentId { get; set; } = string.Empty;

    public string TypeOfEstablishmentName { get; set; } = string.Empty;

    public string EstablishmentTypeGroupId { get; set; } = string.Empty;

    public string EstablishmentTypeGroupName { get; set; } = string.Empty;

    public string ResourcedProvision { get; set; } = string.Empty;

    public string ResourcedProvisionName { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public string Easting { get; set; } = string.Empty;

    public string Northing { get; set; } = string.Empty;

    public int? StatusCode { get; set; }

    public string? ClosedDate { get; set; }

    public string? OpenDate { get; set; }

    public int? OpenReasonId { get; set; }

    public string? SenTypes { get; set; } = string.Empty;

    public bool IsKS2 { get; set; }

    public bool IsKS4 { get; set; }

    public bool IsKS5 { get; set; }

    public bool IsSpecialSchool => new List<string> { "7", "8", "10", "12", "33", "36", "44" }.Contains(TypeOfEstablishmentId);

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

    public static EstablishmentServiceModel Map(Establishment e)
    {
        return new()
        {
            URN = e.URN,
            EstablishmentName = e.EstablishmentName,
            TrustsId = e.TrustsId,
            TrustName = e.TrustName,
            AddressStreet = e.AddressStreet,
            AddressLocality = e.AddressLocality,
            AddressAddress3 = e.AddressAddress3,
            AddressTown = e.AddressTown,
            AddressCounty = e.AddressCounty,
            AddressPostcode = e.AddressPostcode,
            AdmissionsPolicyId = e.AdmissionsPolicyId,
            AdmissionPolicy = e.AdmissionPolicy,
            DistrictAdministrativeId = e.DistrictAdministrativeId,
            DistrictAdministrativeName = e.DistrictAdministrativeName,
            PhaseOfEducationId = e.PhaseOfEducationId,
            PhaseOfEducationName = e.PhaseOfEducationName,
            GenderId = e.GenderId,
            GenderName = e.GenderName,
            HeadteacherTitle = e.HeadteacherTitle,
            HeadteacherFirstName = e.HeadteacherFirstName,
            HeadteacherLastName = e.HeadteacherLastName,
            HeadteacherPreferredJobTitle = e.HeadteacherPreferredJobTitle,
            AgeRangeLow = e.AgeRangeLow,
            AgeRangeHigh = e.AgeRangeHigh,
            OfficialSixthFormId = e.OfficialSixthFormId,
            LAId = e.LAId,
            LAName = e.LAName,
            GSSLACode = e.GSSLACode,
            ReligiousCharacterId = e.ReligiousCharacterId,
            ReligiousCharacterName = e.ReligiousCharacterName,
            TelephoneNum = e.TelephoneNum,
            TotalPupils = e.TotalPupils,
            TypeOfEstablishmentId = e.TypeOfEstablishmentId,
            TypeOfEstablishmentName = e.TypeOfEstablishmentName,
            EstablishmentTypeGroupId = e.EstablishmentTypeGroupId,
            EstablishmentTypeGroupName = e.EstablishmentTypeGroupName,
            ResourcedProvision = e.ResourcedProvision,
            ResourcedProvisionName = e.ResourcedProvisionName,
            Website = e.Website,
            Easting = e.Easting,
            Northing = e.Northing,
            StatusCode = e.StatusCode,
            ClosedDate = e.ClosedDate,
            OpenDate = e.OpenDate,
            OpenReasonId = e.OpenReasonId,
            SenTypes = e.SenTypes,
            IsKS2 = e.IsKS2,
            IsKS4 = e.IsKS4,
            IsKS5 = e.IsKS5,
        };
    }
}