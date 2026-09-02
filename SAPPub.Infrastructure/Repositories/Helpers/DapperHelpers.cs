using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;

namespace SAPPub.Infrastructure.Repositories.Helpers
{
    public static class DapperHelpers
    {
        // -----------------------------
        // Column lists (ONLY what each entity needs)
        // -----------------------------

        private const string EstablishmentColumns = """
          "URN",
          "EstablishmentName",
          "TrustsId",
          "TrustName",
          "AddressStreet",
          "AddressLocality",
          "AddressAddress3",
          "AddressTown",
          "AddressCounty",
          "AddressPostcode",
          "AdmissionsPolicyId",
          "AdmissionPolicy",
          "AgeRangeLow",
          "AgeRangeHigh",
          "DistrictAdministrativeId",
          "DistrictAdministrativeName",
          "PhaseOfEducationId",
          "PhaseOfEducationName",
          "GenderId",
          "GenderName",
          "HeadteacherTitle",
          "HeadteacherFirstName",
          "HeadteacherLastName",
          "HeadteacherPreferredJobTitle",
          "OfficialSixthFormId",
          "LAId",
          "LAName",
          "GSSLACode",
          "ReligiousCharacterId",
          "ReligiousCharacterName",
          "TelephoneNum",
          "TotalPupils",
          "TypeOfEstablishmentId",
          "TypeOfEstablishmentName",
          "EstablishmentTypeGroupId",
          "EstablishmentTypeGroupName",
          "ResourcedProvision",
          "ResourcedProvisionName",
          "UKPRN",
          "UrbanRuralId",
          "UrbanRuralName",
          "Website",
          "Easting",
          "Northing",
          "EstablishmentNumber",
          "TotalCapacity" as "SchoolCapacity",
          "StatusCode",
          "ClosedDate",
          "OpenDate",
          "OpenReasonId",
          "SenTypes",
          "ISKS2",
          "ISKS4",
          "ISKS5",
          "KS2WraparoundCare",
          "FreeBreakfastClubProgramme",
          "NurseryProvisionName"
          """;

        private const string EstablishmentDestinationsColumns = """
          "Id",
          "AllDest_Tot_Est_Current_Pct_Coded",
          "Education_Tot_Est_Current_Pct_Coded",
          "Employment_Tot_Est_Current_Pct_Coded",
          "Apprentice_Tot_Est_Current_Pct_Coded",
          "AllDest_Tot_Est_Previous_Pct_Coded",
          "AllDest_Tot_Est_Previous2_Pct_Coded"
          """;

        // TODO for EnableSecondaryGrade7 feature - add these Grade 7 columns when they're available
        //"EngMaths79_Boy_Est_Current_Pct_Coded",
        //"EngMaths79_Grl_Est_Current_Pct_Coded",
        //"EngMaths79_Tot_Est_Current_Pct_Coded",
        //"EngMaths79_Tot_Est_Previous_Pct_Coded",
        //"EngMaths79_Tot_Est_Previous2_Pct_Coded",
        private const string EstablishmentPerformanceColumns = """
          "Id",
          "Attainment8_Tot_Est_Current_Num_Coded",
          "EngMaths49_Boy_Est_Current_Pct_Coded",
          "EngMaths49_Grl_Est_Current_Pct_Coded",
          "EngMaths49_Tot_Est_Current_Pct_Coded",
          "EngMaths59_Boy_Est_Current_Pct_Coded",
          "EngMaths59_Grl_Est_Current_Pct_Coded",
          "EngMaths59_Tot_Est_Current_Pct_Coded",
          "Attainment8_Tot_Est_Previous_Num_Coded",
          "EngMaths49_Tot_Est_Previous_Pct_Coded",
          "EngMaths59_Tot_Est_Previous_Pct_Coded",
          "Prog8_Tot_Est_Previous_Num_Coded",
          "Prog8_CI_Lower_Est_Previous_Num_Coded",
          "Prog8_CI_Upper_Est_Previous_Num_Coded", 
          "Prog8_Banding_Est_Previous", 
          "Prog8_TotPup_Est_Previous_Num_Coded",
          "Pup_Tot_Est_Previous_Num_Coded",
          "Attainment8_Tot_Est_Previous2_Num_Coded",
          "EngMaths49_Tot_Est_Previous2_Pct_Coded",
          "EngMaths59_Tot_Est_Previous2_Pct_Coded",
          "Prog8_Tot_Est_Previous2_Num_Coded",
          "Prog8_CI_Lower_Est_Previous2_Num_Coded",
          "Prog8_CI_Upper_Est_Previous2_Num_Coded", 
          "Prog8_Banding_Est_Previous2",          
          "Prog8_TotPup_Est_Previous2_Num_Coded",
          "Pup_Tot_Est_Previous2_Num_Coded",
          "AnyQual_Tot_Est_Current_Pct_Coded",
          "TripSci_Tot_Est_Current_Pct_Coded",
          "More1FL_Tot_Est_Current_Pct_Coded",
          "ExamEntriesGSCE_Tot_Est_Current_Num_Coded",
          "ExamEntriesKS4_Tot_Est_Current_Num_Coded",
          "Pup_Tot_Est_Current_Num_Coded"
          """;

        // TODO for EnableSecondaryGrade7 feature - add these Grade 7 columns when they're available
        //"EngMaths79_Boy_LA_Current_Pct_Coded",
        //"EngMaths79_Grl_LA_Current_Pct_Coded",
        //"EngMaths79_Tot_LA_Current_Pct_Coded",
        //"EngMaths79_Tot_LA_Previous_Pct_Coded",
        //"EngMaths79_Tot_LA_Previous2_Pct_Coded",
        private const string LAPerformanceColumns = """
          "Id",
          "Attainment8_Tot_LA_Current_Num_Coded",
          "EngMaths49_Boy_LA_Current_Pct_Coded",
          "EngMaths49_Grl_LA_Current_Pct_Coded",
          "EngMaths49_Tot_LA_Current_Pct_Coded",
          "EngMaths59_Boy_LA_Current_Pct_Coded",
          "EngMaths59_Grl_LA_Current_Pct_Coded",
          "EngMaths59_Tot_LA_Current_Pct_Coded",
          "Attainment8_Tot_LA_Previous_Num_Coded",
          "EngMaths49_Tot_LA_Previous_Pct_Coded",
          "EngMaths59_Tot_LA_Previous_Pct_Coded",
          "Prog8_Avg_LA_Previous_Num_Coded",
          "Attainment8_Tot_LA_Previous2_Num_Coded",
          "EngMaths49_Tot_LA_Previous2_Pct_Coded",
          "EngMaths59_Tot_LA_Previous2_Pct_Coded",
          "Prog8_Avg_LA_Previous2_Num_Coded",
          "AnyQual_Tot_LA_Current_Pct_Coded",
          "TripSci_Tot_LA_Current_Pct_Coded",
          "More1FL_Tot_LA_Current_Pct_Coded",
          "ExamEntriesGSCE_Tot_LA_Current_Num_Coded",
          "ExamEntriesKS4_Tot_LA_Current_Num_Coded",
          "Pup_Tot_LA_Current_Num_Coded"
          """;

        private const string LADestinationsColumns = """
          "Id",
          "AllDest_Tot_LA_Current_Pct_Coded",
          "Education_Tot_LA_Current_Pct_Coded",
          "Employment_Tot_LA_Current_Pct_Coded",
          "Apprentice_Tot_LA_Current_Pct_Coded",
          "AllDest_Tot_LA_Previous_Pct_Coded",
          "AllDest_Tot_LA_Previous2_Pct_Coded"
          """;

        // TODO for EnableSecondaryGrade7 feature - add these Grade 7 columns when they're available
        //"EngMaths79_Boy_Eng_Current_Pct_Coded",
        //"EngMaths79_Grl_Eng_Current_Pct_Coded",
        //"EngMaths79_Tot_Eng_Current_Pct_Coded",
        //"EngMaths79_Tot_Eng_Previous_Pct_Coded",
        //"EngMaths79_Tot_Eng_Previous2_Pct_Coded",
        private const string EnglandPerformanceColumns = """
          "Id",
          "Attainment8_Tot_Eng_Current_Num_Coded",
          "EngMaths49_Boy_Eng_Current_Pct_Coded",
          "EngMaths49_Grl_Eng_Current_Pct_Coded",
          "EngMaths49_Tot_Eng_Current_Pct_Coded",
          "EngMaths59_Boy_Eng_Current_Pct_Coded",
          "EngMaths59_Grl_Eng_Current_Pct_Coded",
          "EngMaths59_Tot_Eng_Current_Pct_Coded",
          "Attainment8_Tot_Eng_Previous_Num_Coded",
          "EngMaths49_Tot_Eng_Previous_Pct_Coded",
          "EngMaths59_Tot_Eng_Previous_Pct_Coded",
          "Attainment8_Tot_Eng_Previous2_Num_Coded",
          "EngMaths49_Tot_Eng_Previous2_Pct_Coded",
          "EngMaths59_Tot_Eng_Previous2_Pct_Coded",
          "AnyQual_Tot_Eng_Current_Pct_Coded",
          "TripSci_Tot_Eng_Current_Pct_Coded",
          "More1FL_Tot_Eng_Current_Pct_Coded",
          "ExamEntriesGSCE_Tot_Eng_Current_Num_Coded",
          "ExamEntriesKS4_Tot_Eng_Current_Num_Coded",
          "Pup_Tot_Eng_Current_Num_Coded"
          """;

        private const string EnglandDestinationsColumns = """
          "Id",
          "AllDest_Tot_Eng_Current_Pct_Coded",
          "Education_Tot_Eng_Current_Pct_Coded",
          "Employment_Tot_Eng_Current_Pct_Coded",
          "Apprentice_Tot_Eng_Current_Pct_Coded",
          "AllDest_Tot_Eng_Previous_Pct_Coded",
          "AllDest_Tot_Eng_Previous2_Pct_Coded"
          """;

        private const string EstablishmentSubjectEntriesColumns = """
          school_urn,
          pupil_count,
          subject,
          subject_discount_group,
          qualification_type,
          qualification_detailed,
          grade,
          number_achieving
          """;

        private const string EstablishmentKs5SubjectEntriesColumns = """
          "subject",
          "qualification_detailed",
          "qualification_level",
          "entries_count",
          "exam_cohort",
          "grade"
          """;

        private const string EnglandAbsenceColumns = """
          "Id",
          "Abs_Persistent_Eng_Current_Pct_Coded",
          "Abs_Tot_Eng_Current_Pct_Coded",
          "Abs_PersistentKS2_Eng_Current_Pct_Coded", 
          "Abs_PersistentSPE_Eng_Current_Pct_Coded", 
          "Abs_TotKS2_Eng_Current_Pct_Coded", 
          "Abs_TotSPE_Eng_Current_Pct_Coded"
          """;

        private const string LaUrlsColumns = """
          "Id",
          "Name",
          "LAMainUrl"
          """;

        private const string KS5EstablishmentPerformanceColumns = """
          "Id",
          "TALLPUP_ALEV_1618_Est_Current_Num_Coded",          
          "VA_INS_ALEV_Est_Current_Num_Coded",
          "PROGRESS_BAND_ALEV_Est_Current",
          "UCI_INS_ALEV_Est_Current_Num_Coded",
          "LCI_INS_ALEV_Est_Current_Num_Coded",
          "TALLPPE_ALEV_1618_Est_Current_Num_Coded",
          "TALLPPEGRD_ALEV_1618_Est_Current",
          "TALLPUP_ACAD_1618_Est_Current_Num_Coded",
          "VA_INS_ACAD_Est_Current_Num_Coded",
          "PROGRESS_BAND_ACAD_Est_Current",
          "UCI_INS_ACAD_Est_Current_Num_Coded",
          "LCI_INS_ACAD_Est_Current_Num_Coded",
          "TALLPPE_ACAD_1618_Est_Current_Num_Coded",
          "TALLPPEGRD_ACAD_1618_Est_Current",          
          "TALLPUP_AGEN_Est_Current_Num_Coded",          
          "VA_INS_AGEN_Est_Current_Num_Coded",
          "PROGRESS_BAND_AGEN_Est_Current",
          "UCI_INS_AGEN_Est_Current_Num_Coded",
          "LCI_INS_AGEN_Est_Current_Num_Coded",
          "TALLPPE_AGEN_Est_Current_Num_Coded",
          "TALLPPEGRD_AGEN_Est_Current",
          "TALLPUP_TLEV_Est_Current_Num_Coded",
          "VA_INS_TLEV_Est_Current_Num_Coded",
          "PROGRESS_BAND_TLEV_Est_Current",
          "UCI_INS_TLEV_Est_Current_Num_Coded",
          "LCI_INS_TLEV_Est_Current_Num_Coded",
          "TALLPPE_TLEV_Est_Current_Num_Coded",
          "TALLPPEGRD_TLEV_Est_Current",
          "TALLPUP_TECHCERT_Est_Current_Num_Coded",
          "VA_INS_TECHCERT_Est_Current_Num_Coded",
          "PROGRESS_BAND_TECHCERT_Est_Current",
          "UCI_INS_TECHCERT_Est_Current_Num_Coded",
          "LCI_INS_TECHCERT_Est_Current_Num_Coded",
          "TALLPPE_TECHCERT_Est_Current_Num_Coded",
          "TALLPPEGRD_TECHCERT_Est_Current",
          "TINCLUDE_B3_Est_Current_Num_Coded",
          "TB3PTSE_Est_Current_Num_Coded",
          "TB3PTSE_GRD_Est_Current",
          "L3M_PER_Est_Current_Pct_Coded",
          "T_SCOPEEX_E_Est_Current_Num_Coded",
          "PROGEX_E_Est_Current_Num_Coded",
          "ENTRY_PER_E_Est_Current_Pct_Coded",
          "T_SCOPEEX_M_Est_Current_Num_Coded",
          "PROGEX_M_Est_Current_Num_Coded",
          "ENTRY_PER_M_Est_Current_Pct_Coded",
          "T_SCOPEEX_E_DIS_Est_Current_Num_Coded",
          "PROGEX_E_DIS_Est_Current_Num_Coded",
          "T_SCOPEEX_M_DIS_Est_Current_Num_Coded",
          "PROGEX_M_DIS_Est_Current_Num_Coded",
          "TALLPUP_ALEV_1618_DIS_Est_Current_Num_Coded",
          "VA_INS_ALEV_DIS_Est_Current_Num_Coded",
          "UCI_INS_ALEV_DIS_Est_Current_Num_Coded",
          "LCI_INS_ALEV_DIS_Est_Current_Num_Coded",
          "TALLPPEGRD_ALEV_DIS_Est_Current",
          "TALLPPE_ALEV_1618_DIS_Est_Current_Num_Coded",
          "TALLPUP_ACAD_1618_DIS_Est_Current_Num_Coded",
          "VA_INS_ACAD_DIS_Est_Current_Num_Coded",
          "UCI_INS_ACAD_DIS_Est_Current_Num_Coded",
          "LCI_INS_ACAD_DIS_Est_Current_Num_Coded",
          "TALLPPEGRD_ACAD_DIS_Est_Current",
          "TALLPPE_ACAD_1618_DIS_Est_Current_Num_Coded",
          "TALLPUP_AGEN_DIS_Est_Current_Num_Coded",
          "VA_INS_AGEN_DIS_Est_Current_Num_Coded",
          "UCI_INS_AGEN_DIS_Est_Current_Num_Coded",
          "LCI_INS_AGEN_DIS_Est_Current_Num_Coded",
          "TALLPPEGRD_AGEN_DIS_Est_Current",
          "TALLPPE_AGEN_DIS_Est_Current_Num_Coded"          
          """;

        private const string KS5EnglandPerformanceColumns = """
          "Id",
          "VA_INS_ALEV_Eng_Current_Num_Coded",
          "TALLPPE_ALEV_1618_Eng_Current_Num_Coded",
          "TALLPPEGRD_ALEV_1618_Eng_Current",
          "VA_INS_ACAD_Eng_Current_Num_Coded",
          "TALLPPE_ACAD_1618_Eng_Current_Num_Coded",
          "TALLPPEGRD_ACAD_1618_Eng_Current",
          "VA_INS_AGEN_Eng_Current_Num_Coded",
          "TALLPPE_AGEN_Eng_Current_Num_Coded",
          "TALLPPEGRD_AGEN_Eng_Current",
          "VA_INS_TLEV_Eng_Current_Num_Coded",
          "TALLPPE_TLEV_Eng_Current_Num_Coded",
          "TALLPPEGRD_TLEV_Eng_Current",
          "VA_INS_TECHCERT_Eng_Current_Num_Coded",
          "TALLPPE_TECHCERT_Eng_Current_Num_Coded",
          "TALLPPEGRD_TECHCERT_Eng_Current",
          "TB3PTSE_Eng_Current_Num_Coded",
          "TB3PTSE_GRD_Eng_Current",
          "L3M_PER_Eng_Current_Pct_Coded",
          "PROGEX_E_Eng_Current_Num_Coded",
          "ENTRY_PER_E_Eng_Current_Pct_Coded",
          "PROGEX_M_Eng_Current_Num_Coded",
          "ENTRY_PER_M_Eng_Current_Pct_Coded",
          "PROGEX_E_DIS_Eng_Current_Num_Coded",
          "PROGEX_E_NOTDIS_Eng_Current_Num_Coded",
          "PROGEX_M_DIS_Eng_Current_Num_Coded",
          "PROGEX_M_NOTDIS_Eng_Current_Num_Coded",
          "T_SCOPEEX_E_DIS_Eng_Current_Num_Coded",
          "T_SCOPEEX_E_NOTDIS_Eng_Current_Num_Coded",
          "T_SCOPEEX_M_DIS_Eng_Current_Num_Coded",
          "T_SCOPEEX_M_NOTDIS_Eng_Current_Num_Coded",
          "TALLPUP_ALEV_1618_DIS_Eng_Current_Num_Coded",
          "VA_INS_ALEV_DIS_Eng_Current_Num_Coded",
          "UCI_INS_ALEV_DIS_Eng_Current_Num_Coded",
          "LCI_INS_ALEV_DIS_Eng_Current_Num_Coded",
          "TALLPPEGRD_ALEV_DIS_Eng_Current",
          "TALLPPE_ALEV_1618_DIS_Eng_Current_Num_Coded",
          "TALLPUP_ALEV_1618_NOTDIS_Eng_Current_Num_Coded",
          "VA_INS_ALEV_NOTDIS_Eng_Current_Num_Coded",
          "UCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded",
          "LCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded",
          "TALLPPEGRD_ALEV_NOTDIS_Eng_Current",
          "TALLPPE_ALEV_1618_NOTDIS_Eng_Current_Num_Coded",
          "TALLPUP_ACAD_1618_DIS_Eng_Current_Num_Coded",
          "VA_INS_ACAD_DIS_Eng_Current_Num_Coded",
          "UCI_INS_ACAD_DIS_Eng_Current_Num_Coded",
          "LCI_INS_ACAD_DIS_Eng_Current_Num_Coded",
          "TALLPPEGRD_ACAD_DIS_Eng_Current",
          "TALLPPE_ACAD_1618_DIS_Eng_Current_Num_Coded",
          "TALLPUP_ACAD_1618_NOTDIS_Eng_Current_Num_Coded",
          "VA_INS_ACAD_NOTDIS_Eng_Current_Num_Coded",
          "UCI_INS_ACAD_NOTDIS_Eng_Current_Num_Coded",
          "LCI_INS_ACAD_NOTDIS_Eng_Current_Num_Coded",
          "TALLPPEGRD_ACAD_NOTDIS_Eng_Current",
          "TALLPPE_ACAD_1618_NOTDIS_Eng_Current_Num_Coded",
          "TALLPUP_AGEN_DIS_Eng_Current_Num_Coded",
          "VA_INS_AGEN_DIS_Eng_Current_Num_Coded",
          "UCI_INS_AGEN_DIS_Eng_Current_Num_Coded",
          "LCI_INS_AGEN_DIS_Eng_Current_Num_Coded",
          "TALLPPEGRD_AGEN_DIS_Eng_Current",
          "TALLPPE_AGEN_DIS_Eng_Current_Num_Coded",
          "TALLPUP_AGEN_NOTDIS_Eng_Current_Num_Coded",
          "VA_INS_AGEN_NOTDIS_Eng_Current_Num_Coded",
          "UCI_INS_AGEN_NOTDIS_Eng_Current_Num_Coded",
          "LCI_INS_AGEN_NOTDIS_Eng_Current_Num_Coded",
          "TALLPPEGRD_AGEN_NOTDIS_Eng_Current",
          "TALLPPE_AGEN_NOTDIS_Eng_Current_Num_Coded"
          """;

        private const string KS5LAPerformanceColumns = """
          "Id",
          "TALLPPE_ALEV_1618_LA_Current_Num_Coded",
          "TALLPPEGRD_ALEV_1618_LA_Current",
          "TALLPPE_ACAD_1618_LA_Current_Num_Coded",
          "TALLPPEGRD_ACAD_1618_LA_Current",
          "TALLPPE_AGEN_LA_Current_Num_Coded",
          "TALLPPEGRD_AGEN_LA_Current",
          "TALLPPE_TLEV_LA_Current_Num_Coded",
          "TALLPPEGRD_TLEV_LA_Current",
          "TALLPPE_TECHCERT_LA_Current_Num_Coded",
          "TALLPPEGRD_TECHCERT_LA_Current",
          "TB3PTSE_LA_Current_Num_Coded",
          "TB3PTSE_GRD_LA_Current",
          "L3M_PER_LA_Current_Pct_Coded",
          "PROGEX_E_LA_Current_Num_Coded",
          "ENTRY_PER_E_LA_Current_Pct_Coded",
          "PROGEX_M_LA_Current_Num_Coded",
          "ENTRY_PER_M_LA_Current_Pct_Coded",
          "PROGEX_E_DIS_LA_Current_Num_Coded",
          "PROGEX_E_NOTDIS_LA_Current_Num_Coded",
          "PROGEX_M_DIS_LA_Current_Num_Coded",
          "PROGEX_M_NOTDIS_LA_Current_Num_Coded",
          "T_SCOPEEX_E_DIS_LA_Current_Num_Coded",
          "T_SCOPEEX_E_NOTDIS_LA_Current_Num_Coded",
          "T_SCOPEEX_M_DIS_LA_Current_Num_Coded",
          "T_SCOPEEX_M_NOTDIS_LA_Current_Num_Coded",
          "TALLPUP_ALEV_1618_DIS_LA_Current_Num_Coded",
          "VA_INS_ALEV_DIS_LA_Current_Num_Coded",
          "UCI_INS_ALEV_DIS_LA_Current_Num_Coded",
          "LCI_INS_ALEV_DIS_LA_Current_Num_Coded",
          "TALLPPEGRD_ALEV_DIS_LA_Current",
          "TALLPPE_ALEV_1618_DIS_LA_Current_Num_Coded",
          "TALLPUP_ALEV_1618_NOTDIS_LA_Current_Num_Coded",
          "VA_INS_ALEV_NOTDIS_LA_Current_Num_Coded",
          "UCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded",
          "LCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded",
          "TALLPPEGRD_ALEV_NOTDIS_LA_Current",
          "TALLPPE_ALEV_1618_NOTDIS_LA_Current_Num_Coded",
          "TALLPUP_ACAD_1618_DIS_LA_Current_Num_Coded",
          "VA_INS_ACAD_DIS_LA_Current_Num_Coded",
          "UCI_INS_ACAD_DIS_LA_Current_Num_Coded",
          "LCI_INS_ACAD_DIS_LA_Current_Num_Coded",
          "TALLPPEGRD_ACAD_DIS_LA_Current",
          "TALLPPE_ACAD_1618_DIS_LA_Current_Num_Coded",
          "TALLPUP_ACAD_1618_NOTDIS_LA_Current_Num_Coded",
          "VA_INS_ACAD_NOTDIS_LA_Current_Num_Coded",
          "UCI_INS_ACAD_NOTDIS_LA_Current_Num_Coded",
          "LCI_INS_ACAD_NOTDIS_LA_Current_Num_Coded",
          "TALLPPEGRD_ACAD_NOTDIS_LA_Current",
          "TALLPPE_ACAD_1618_NOTDIS_LA_Current_Num_Coded",
          "TALLPUP_AGEN_DIS_LA_Current_Num_Coded",
          "VA_INS_AGEN_DIS_LA_Current_Num_Coded",
          "UCI_INS_AGEN_DIS_LA_Current_Num_Coded",
          "LCI_INS_AGEN_DIS_LA_Current_Num_Coded",
          "TALLPPEGRD_AGEN_DIS_LA_Current",
          "TALLPPE_AGEN_DIS_LA_Current_Num_Coded",
          "TALLPUP_AGEN_NOTDIS_LA_Current_Num_Coded",
          "VA_INS_AGEN_NOTDIS_LA_Current_Num_Coded",
          "UCI_INS_AGEN_NOTDIS_LA_Current_Num_Coded",
          "LCI_INS_AGEN_NOTDIS_LA_Current_Num_Coded",
          "TALLPPEGRD_AGEN_NOTDIS_LA_Current",
          "TALLPPE_AGEN_NOTDIS_LA_Current_Num_Coded"
          """;

        private const string GatewayLAColumns = """
          "Id",
          "LocalAuthorityName",
          "MaxSessions",
          "CreatedOn",
          "ModifiedOn",
          "IsDeleted"
          """;

        private const string GatewaySettings = """
          "Id",
          "SettingName",
          "SettingValue",
          "CreatedOn",
          "ModifiedOn",
          "IsDeleted"
          """;

        private const string GatewayUser = """
          "Id",
          "EmailAddress",
          "LocalAuthorityId",
          "CookiePrefs",
          "TimerStartedOn",
          "CreatedOn",
          "ModifiedOn",
          "IsDeleted"
          """;

        // -----------------------------
        // SQL builders
        // -----------------------------

        private static string SelectFrom(string columns, string viewName) => $"""
            select
              {columns}
            from public.{viewName};
            """;

        private static string SelectFromAndNotDeleted(string columns, string viewName) => $"""
            select
              {columns}
            from public.{viewName} where "IsDeleted" = false;
            """;

        private static string SelectFromWhereId(string columns, string viewName) => $"""
            select
              {columns}
            from public.{viewName}
            where "Id" = @Id;
            """;

        private static string SelectAllFromWhereId(string viewName) => $"""
            select *
            from public.{viewName}
            where "Id" = @Id;
            """;

        private static string SelectFromWhereIds(string columns, string viewName) => $"""
            select
              {columns}
            from public.{viewName}
            where "Id" = ANY(@Ids);
            """;

        private static string SelectFromWhereIdAndNotDeleted(string columns, string viewName) => $"""
            select
              {columns}
            from public.{viewName}
            where "Id" = @Id and "IsDeleted" = false;
            """;

        // Establishment uses URN
        private static string SelectFromWhereUrn(string columns, string viewName) => $"""
            select
              {columns}
            from public.{viewName}
            where "URN" = @Id;
            """;

        private static string SelectFromWhereUrns(string columns, string viewName) => $"""
            select
              {columns}
            from public.{viewName}
            where "URN" = ANY(@Urns);
            """;

        private static string SelectFromWhereGSSLACode(string columns, string viewName) => $"""
            select
              {columns}
            from public.{viewName}
            where "GSSLACode" = ANY(@GSSLaCodes);
            """;

        private static string SelectFromWhere(string columns, string view, string where)
        {
            return $"""
        select
          {columns}
        from public.{view} 
        where {where};
        """;
        }

        private static string SelectAllFromWhere(string view, string where)
        {
            return $"""
        select *
        from public.{view} 
        where {where};
        """;
        }

        // -----------------------------
        // Public API
        // -----------------------------

        public static string GetReadMultiple(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(Establishment) => $"""
                    select
                      {EstablishmentColumns}
                    from public.v_establishment
                    """ + DapperHelpers.GetOrderBy(typeof(Establishment)),

                nameof(EstablishmentAbsence) =>
                    SelectAllFromWhereId("v_establishment_absence"),

                nameof(KS4EstablishmentDestinations) =>
                    SelectFrom(EstablishmentDestinationsColumns, "v_establishment_destinations"),

                nameof(EstablishmentPerformance) =>
                    SelectFrom(EstablishmentPerformanceColumns, "v_establishment_performance"),

                nameof(LAAbsence) =>
                    SelectAllFromWhereId("v_la_absence"),

                nameof(KS4LADestinations) =>
                    SelectFrom(LADestinationsColumns, "v_la_destinations"),

                nameof(LAPerformance) =>
                    SelectFrom(LAPerformanceColumns, "v_la_performance"),

                nameof(EnglandAbsence) =>
                    SelectFrom(EnglandAbsenceColumns, "v_england_absence"),

                nameof(KS4EnglandDestinations) =>
                    SelectFrom(EnglandDestinationsColumns, "v_england_destinations"),

                nameof(EnglandPerformance) =>
                    SelectFrom(EnglandPerformanceColumns, "v_england_performance"),

                nameof(LaUrls) =>
                    SelectFrom(LaUrlsColumns, "v_la_urls"),

                nameof(GatewayLocalAuthority) =>
                    SelectFromAndNotDeleted(GatewayLAColumns, "gateway_local_authority"),

                nameof(GatewaySettings) =>
                    SelectFromAndNotDeleted(GatewaySettings, "gateway_settings"),

                nameof(GatewayUser) =>
                    SelectFromAndNotDeleted(GatewayUser, "gateway_user"),

                _ => string.Empty,
            };
        }

        public static string GetOrderBy(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(Establishment) =>
                    " ORDER BY \"EstablishmentName\"",

                _ => string.Empty,
            };
        }

        public static string GetReadSingle(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(Establishment) =>
                    SelectFromWhereUrn(EstablishmentColumns, "v_establishment"),

                nameof(EstablishmentAbsence) =>
                    SelectAllFromWhereId("v_establishment_absence"),

                nameof(KS4EstablishmentDestinations) =>
                    SelectFromWhereId(EstablishmentDestinationsColumns, "v_establishment_destinations"),

                nameof(EstablishmentPerformance) =>
                    SelectFromWhereId(EstablishmentPerformanceColumns, "v_establishment_performance"),

                nameof(LAAbsence) =>
                    SelectAllFromWhereId("v_la_absence"),

                nameof(KS4LADestinations) =>
                    SelectFromWhereId(LADestinationsColumns, "v_la_destinations"),

                nameof(LAPerformance) =>
                    SelectFromWhereId(LAPerformanceColumns, "v_la_performance"),

                nameof(EnglandAbsence) =>
                    SelectFromWhere(EnglandAbsenceColumns, "v_england_absence", "\"Id\" = 'National'"),

                nameof(KS4EnglandDestinations) =>
                    SelectFromWhere(EnglandDestinationsColumns, "v_england_destinations", "\"Id\" = 'National'"),

                nameof(EnglandPerformance) =>
                    SelectFromWhere(EnglandPerformanceColumns, "v_england_performance", "\"Id\" = 'National'"),

                nameof(LaUrls) =>
                    SelectFromWhereId(LaUrlsColumns, "v_la_urls"),

                nameof(KS5EstablishmentPerformance) =>
                    SelectFromWhereId(KS5EstablishmentPerformanceColumns, "v_establishment_ks5_performance"),

                nameof(KS2EstablishmentPerformance) =>
                    SelectAllFromWhereId("v_establishment_ks2_attainment"),

                nameof(KS2LAPerformance) =>
                    SelectAllFromWhereId("v_la_ks2_attainment"),

                nameof(KS2EnglandPerformance) =>
                    SelectAllFromWhere("v_england_ks2_attainment", "\"Id\" = 'National'"),

                nameof(KS5EnglandPerformance) =>
                    SelectFromWhere(KS5EnglandPerformanceColumns, "v_england_ks5_performance", "\"Id\" = 'National'"),

                nameof(KS5LAPerformance) =>
                    SelectFromWhereId(KS5LAPerformanceColumns, "v_la_ks5_performance"),

                nameof(GatewayLocalAuthority) =>
                    SelectFromWhereIdAndNotDeleted(GatewayLAColumns, "gateway_local_authority"),

                nameof(GatewaySettings) =>
                    SelectFromWhereIdAndNotDeleted(GatewaySettings, "gateway_settings"),

                nameof(GatewayUser) =>
                    SelectFromWhereIdAndNotDeleted(GatewayUser, "gateway_user"),

                nameof(KS5EstablishmentDestinations) => SelectFromWhereId("\"Id\", \"TOT_OVERALLPER_Est_Current_Pct_Coded\", \"TOT_COHORT_Est_Current_Num_Coded\"", "v_establishment_ks5_destinations"),
                nameof(KS5EnglandDestinations) => SelectFromWhere("\"TOT_OVERALLPER_Eng_Current_Pct_Coded\"", "v_england_ks5_destinations", "\"Id\" = 'National'"),
                nameof(KS5LADestinations) => SelectFromWhereId("\"Id\", \"TOT_OVERALLPER_LA_Current_Pct_Coded\"", "v_la_ks5_destinations"),

                _ => string.Empty,
            };
        }

        // Writes will be removed when Gateway is no longer needed, direct SQL (with dapper parameters) should be easy and safe enough. 
        public static string GetWriteSingle(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(GatewayUser) =>
                    $"INSERT INTO \"gateway_user\" (  \"Id\",  \"EmailAddress\",  \"LocalAuthorityId\",  \"CookiePrefs\",  \"TimerStartedOn\",  \"CreatedOn\",  \"ModifiedOn\",  \"IsDeleted\") VALUES (  @Id,  @EmailAddress,  @LocalAuthorityId,  @CookiePrefs,  @TimerStartedOn,  @CreatedOn,  @ModifiedOn,  @IsDeleted);",

                nameof(GatewayUserAudit) =>
                    $"INSERT INTO \"gateway_user_audit\" (  \"Id\",  \"UserId\",  \"LoginDateTime\", \"UserAction\", \"CreatedOn\",  \"ModifiedOn\", \"IsDeleted\" )VALUES (  @Id,  @UserId,  @LoginDateTime, @UserAction, @CreatedOn,  @ModifiedOn,  @IsDeleted);",

                nameof(GatewayLocalAuthority) =>
                    "INSERT INTO \"gateway_local_authority\" (  \"Id\",  \"LocalAuthorityName\",  \"MaxSessions\",  \"CreatedOn\",  \"ModifiedOn\",  \"IsDeleted\" )VALUES (  @Id,  @LocalAuthorityName,  @MaxSessions,  @CreatedOn,  @ModifiedOn, @IsDeleted);",

                nameof(GatewaySettings) =>
                    "INSERT INTO \"gateway_settings\" (  \"Id\",  \"Key\",  \"Value\",  \"CreatedOn\",  \"ModifiedOn\",  \"IsDeleted\")VALUES (  @Id,  @Key,  @Value,  @CreatedOn,  @ModifiedOn, @IsDeleted);",

                _ => string.Empty,
            };
        }

        // Updates will be removed when Gateway is no longer needed, direct SQL (with dapper parameters) should be easy and safe enough. 
        public static string GetUpdateSingle(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(GatewayUser) =>
                    $"UPDATE gateway_user SET \"EmailAddress\" = @EmailAddress,    \"LocalAuthorityId\" = @LocalAuthorityId,    \"CookiePrefs\" = @CookiePrefs,    \"TimerStartedOn\" = @TimerStartedOn,    \"CreatedOn\" = @CreatedOn,    \"ModifiedOn\" = @ModifiedOn,  \"IsDeleted\" = @IsDeleted WHERE \"Id\" = @Id;",

                nameof(GatewayUserAudit) =>
                    $"UPDATE gateway_user_audit SET \"UserId\"=@UserId, \"LoginDateTime\"=@LoginDateTime, \"UserAction\"=@UserAction, \"CreatedOn\"=@CreatedOn, \"ModifiedOn\"=@ModifiedOn,  \"IsDeleted\"=@IsDeleted WHERE \"Id\"=@Id;",

                nameof(GatewayLocalAuthority) =>
                    "UPDATE gateway_local_authority SET \"LocalAuthorityName\"=@LocalAuthorityName, \"MaxSessions\"=@MaxSessions, \"CreatedOn\"=@CreatedOn, \"ModifiedOn\"=@ModifiedOn,  \"IsDeleted\"=@IsDeleted WHERE \"Id\"=@Id;",

                nameof(GatewaySettings) =>
                    "UPDATE gateway_settings SET \"SettingName\"=@SettingName, \"SettingValue\"=@SettingValue, \"CreatedOn\"=@CreatedOn, \"ModifiedOn\"=@ModifiedOn,  \"IsDeleted\"=@IsDeleted WHERE \"Id\"=@Id;",

                _ => string.Empty,
            };
        }

        public static string GetReadMany(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(Establishment) =>
                    SelectFromWhereUrns(EstablishmentColumns, "v_establishment"),

                nameof(EstablishmentPerformance) =>
                    SelectFromWhereIds(EstablishmentPerformanceColumns, "v_establishment_performance"),

                nameof(KS4EstablishmentSubjectEntryRow) => $"""
                    select
                      {EstablishmentSubjectEntriesColumns}
                    from public.v_establishment_subject_entries
                    where school_urn = @Urn;
                    """,

                nameof(KS5EstablishmentSubjectEntryRow) => $"""
                    select
                      {EstablishmentKs5SubjectEntriesColumns}
                    from public.v_establishment_ks5_subject_entries
                    where school_urn = @Urn;
                    """,

                nameof(KS4EstablishmentDestinations) =>
                    SelectFromWhereIds(EstablishmentDestinationsColumns, "v_establishment_destinations"),

                nameof(LaUrls) =>
                    SelectFromWhereGSSLACode(LaUrlsColumns, "v_la_urls"),

                _ => string.Empty,
            };
        }
    }
}