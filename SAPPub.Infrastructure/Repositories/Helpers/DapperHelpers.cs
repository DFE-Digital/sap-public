using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Entities.KS4.Workforce;

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
          "UKPRN",
          "UrbanRuralId",
          "UrbanRuralName",
          "Website",
          "Easting",
          "Northing",
          "EstablishmentNumber",
          "TotalCapacity" as "SchoolCapacity",
          "StatusCode"
          """;

        private const string EstablishmentAbsenceColumns = """
          "Id",
          "Abs_Persistent_Est_Current_Pct_Coded",
          "Abs_Persistent_Est_Previous_Pct_Coded",
          "Abs_Persistent_Est_Previous2_Pct_Coded",
          "Abs_Tot_Est_Current_Pct_Coded",
          "Abs_Tot_Est_Previous_Pct_Coded",
          "Abs_Tot_Est_Previous2_Pct_Coded",
          "Auth_Tot_Est_Current_Pct_Coded",
          "UnAuth_Tot_Est_Current_Pct_Coded"
          """;

        private const string EstablishmentDestinationsColumns = """
          "Id",
          "AllDest_Tot_Est_Current_Pct_Coded",
          "Education_Tot_Est_Current_Pct_Coded",
          "Employment_Tot_Est_Current_Pct_Coded",
          "Apprentice_Tot_Est_Current_Pct_Coded",
          "AllDest_Tot_Est_Previous_Pct_Coded",
          "Education_Tot_Est_Previous_Pct_Coded",
          "Employment_Tot_Est_Previous_Pct_Coded",
          "Apprentice_Tot_Est_Previous_Pct_Coded",
          "AllDest_Tot_Est_Previous2_Pct_Coded",
          "Education_Tot_Est_Previous2_Pct_Coded",
          "Employment_Tot_Est_Previous2_Pct_Coded",
          "Apprentice_Tot_Est_Previous2_Pct_Coded"
          """;

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
          "Prog8_TotPup_Est_Previous_Num_Coded",
          "Pup_Tot_Est_Previous_Num_Coded",
          "Attainment8_Tot_Est_Previous2_Num_Coded",
          "EngMaths49_Tot_Est_Previous2_Pct_Coded",
          "EngMaths59_Tot_Est_Previous2_Pct_Coded",
          "Prog8_Tot_Est_Previous2_Num_Coded",
          "Prog8_TotPup_Est_Previous2_Num_Coded",
          "Pup_Tot_Est_Previous2_Num_Coded"
          """;

        private const string EstablishmentWorkforceColumns = """
          "Id",
          "Workforce_PupTeaRatio_Est_Current_Num_Coded",
          "Workforce_TotPupils_Est_Current_Num_Coded"
          """;

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
          "Prog8_Avg_LA_Previous2_Num_Coded"
          """;

        private const string LADestinationsColumns = """
          "Id",
          "AllDest_Tot_LA_Current_Pct_Coded",
          "Education_Tot_LA_Current_Pct_Coded",
          "Employment_Tot_LA_Current_Pct_Coded",
          "Apprentice_Tot_LA_Current_Pct_Coded",
          "AllDest_Tot_LA_Previous_Pct_Coded",
          "Education_Tot_LA_Previous_Pct_Coded",
          "Employment_Tot_LA_Previous_Pct_Coded",
          "Apprentice_Tot_LA_Previous_Pct_Coded",
          "AllDest_Tot_LA_Previous2_Pct_Coded",
          "Education_Tot_LA_Previous2_Pct_Coded",
          "Employment_Tot_LA_Previous2_Pct_Coded",
          "Apprentice_Tot_LA_Previous2_Pct_Coded"
          """;

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
          "EngMaths59_Tot_Eng_Previous2_Pct_Coded"
          """;

        private const string EnglandDestinationsColumns = """
          "Id",
          "AllDest_Tot_Eng_Current_Pct_Coded",
          "Education_Tot_Eng_Current_Pct_Coded",
          "Employment_Tot_Eng_Current_Pct_Coded",
          "Apprentice_Tot_Eng_Current_Pct_Coded",
          "AllDest_Tot_Eng_Previous_Pct_Coded",
          "Education_Tot_Eng_Previous_Pct_Coded",
          "Employment_Tot_Eng_Previous_Pct_Coded",
          "Apprentice_Tot_Eng_Previous_Pct_Coded",
          "AllDest_Tot_Eng_Previous2_Pct_Coded",
          "Education_Tot_Eng_Previous2_Pct_Coded",
          "Employment_Tot_Eng_Previous2_Pct_Coded",
          "Apprentice_Tot_Eng_Previous2_Pct_Coded"
          """;

        private const string EstablishmentSubjectEntriesColumns = """
          school_urn,
          pupil_count,
          subject,
          qualification_type,
          qualification_detailed,
          grade,
          number_achieving
          """;

        private const string LaUrlsColumns = """
          "Id",
          "Name",
          "LAMainUrl"
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
          "RegisteredOn",
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

        private static string SelectFromWhere(string columns, string view, string where)
        {
            return $"""
        select
          {columns}
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
                    WHERE "PhaseOfEducationId" in (4, 5, 7)
                    """ + DapperHelpers.GetOrderBy(typeof(Establishment)),

                nameof(EstablishmentAbsence) =>
                    SelectFrom(EstablishmentAbsenceColumns, "v_establishment_absence"),

                nameof(EstablishmentDestinations) =>
                    SelectFrom(EstablishmentDestinationsColumns, "v_establishment_destinations"),

                nameof(EstablishmentPerformance) =>
                    SelectFrom(EstablishmentPerformanceColumns, "v_establishment_performance"),

                nameof(EstablishmentWorkforce) =>
                    SelectFrom(EstablishmentWorkforceColumns, "v_establishment_workforce"),

                nameof(LADestinations) =>
                    SelectFrom(LADestinationsColumns, "v_la_destinations"),

                nameof(LAPerformance) =>
                    SelectFrom(LAPerformanceColumns, "v_la_performance"),

                nameof(EnglandDestinations) =>
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
                    SelectFromWhereId(EstablishmentAbsenceColumns, "v_establishment_absence"),

                nameof(EstablishmentDestinations) =>
                    SelectFromWhereId(EstablishmentDestinationsColumns, "v_establishment_destinations"),

                nameof(EstablishmentPerformance) =>
                    SelectFromWhereId(EstablishmentPerformanceColumns, "v_establishment_performance"),

                nameof(EstablishmentWorkforce) =>
                    SelectFromWhereId(EstablishmentWorkforceColumns, "v_establishment_workforce"),

                nameof(LADestinations) =>
                    SelectFromWhereId(LADestinationsColumns, "v_la_destinations"),

                nameof(LAPerformance) =>
                    SelectFromWhereId(LAPerformanceColumns, "v_la_performance"),

                nameof(EnglandDestinations) =>
                    SelectFromWhere(EnglandDestinationsColumns, "v_england_destinations", "\"Id\" = 'National'"),

                nameof(EnglandPerformance) =>
                    SelectFromWhere(EnglandPerformanceColumns, "v_england_performance", "\"Id\" = 'National'"),

                nameof(LaUrls) =>
                    SelectFromWhereId(LaUrlsColumns, "v_la_urls"),

                nameof(GatewayLocalAuthority) =>
                    SelectFromWhereIdAndNotDeleted(GatewayLAColumns, "gateway_local_authority"),

                nameof(GatewaySettings) =>
                    SelectFromWhereIdAndNotDeleted(GatewaySettings, "gateway_settings"),

                nameof(GatewayUser) =>
                    SelectFromWhereIdAndNotDeleted(GatewayUser, "gateway_user"),

                _ => string.Empty,
            };
        }

        // Writes will be removed when Gateway is no longer needed, direct SQL (with dapper parameters) should be easy and safe enough. 
        public static string GetWriteSingle(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(GatewayUser) => 
                    $"INSERT INTO \"gateway_user\" (  \"Id\",  \"EmailAddress\",  \"LocalAuthorityId\",  \"CookiePrefs\",  \"RegisteredOn\",  \"CreatedOn\",  \"ModifiedOn\",  \"IsDeleted\") VALUES (  @Id,  @EmailAddress,  @LocalAuthorityId,  @CookiePrefs,  @RegisteredOn,  @CreatedOn,  @ModifiedOn,  @IsDeleted);",

                nameof(GatewayUserAudit) => 
                    $"INSERT INTO \"gateway_user_audit\" (  \"Id\",  \"UserId\",  \"LoginDateTime\",  \"CreatedOn\",  \"ModifiedOn\", \"IsDeleted\" )VALUES (  @Id,  @UserId,  @LoginDateTime,  @CreatedOn,  @ModifiedOn,  @IsDeleted);",

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
                    $"UPDATE gateway_user SET \"EmailAddress\" = @EmailAddress,    \"LocalAuthorityId\" = @LocalAuthorityId,    \"CookiePrefs\" = @CookiePrefs,    \"RegisteredOn\" = @RegisteredOn,    \"CreatedOn\" = @CreatedOn,    \"ModifiedOn\" = @ModifiedOn,  \"IsDeleted\" = @IsDeleted WHERE \"Id\" = @Id;",

                nameof(GatewayUserAudit) => 
                    $"UPDATE gateway_user_audit SET \"UserId\"=@UserId, \"LoginDateTime\"=@LoginDateTime, \"CreatedOn\"=@CreatedOn, \"ModifiedOn\"=@ModifiedOn,  \"IsDeleted\"=@IsDeleted WHERE \"Id\"=@Id;",

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
                nameof(EstablishmentSubjectEntryRow) => $"""
                    select
                      {EstablishmentSubjectEntriesColumns}
                    from public.v_establishment_subject_entries
                    where school_urn = @Urn;
                    """,

                _ => string.Empty,
            };
        }
    }
}
