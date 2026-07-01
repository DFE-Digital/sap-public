using SAPPub.Core.ApplicationServices;
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

        // -----------------------------
        // Public API
        // -----------------------------

        public static string GetReadMultiple(Type entityType, IEntityPropertyService entityPropertyService)
        {
            var columnNames = entityPropertyService.GetColumnNamesForType(entityType);
            return entityType.Name switch
            {
                nameof(Establishment) => $"""
                    select
                      {columnNames}
                    from public.v_establishment
                    """ + DapperHelpers.GetOrderBy(typeof(Establishment)),

                nameof(EstablishmentAbsence) =>
                    SelectFrom(columnNames, "v_establishment_absence"),

                nameof(EstablishmentDestinations) =>
                    SelectFrom(columnNames, "v_establishment_destinations"),

                nameof(EstablishmentPerformance) =>
                    SelectFrom(columnNames, "v_establishment_performance"),

                nameof(EstablishmentWorkforce) =>
                    SelectFrom(columnNames, "v_establishment_workforce"),

                nameof(LAAbsence) =>
                    SelectFrom(columnNames, "v_la_absence"),

                nameof(LADestinations) =>
                    SelectFrom(columnNames, "v_la_destinations"),

                nameof(LAPerformance) =>
                    SelectFrom(columnNames, "v_la_performance"),

                nameof(EnglandAbsence) =>
                    SelectFrom(columnNames, "v_england_absence"),

                nameof(EnglandDestinations) =>
                    SelectFrom(columnNames, "v_england_destinations"),

                nameof(EnglandPerformance) =>
                    SelectFrom(columnNames, "v_england_performance"),

                nameof(LaUrls) =>
                    SelectFrom(columnNames, "v_la_urls"),

                nameof(GatewayLocalAuthority) =>
                    SelectFromAndNotDeleted(columnNames, "gateway_local_authority"),

                nameof(GatewaySettings) =>
                    SelectFromAndNotDeleted(columnNames, "gateway_settings"),

                nameof(GatewayUser) =>
                    SelectFromAndNotDeleted(columnNames, "gateway_user"),

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

        public static string GetReadSingle(Type entityType, IEntityPropertyService entityPropertyService)
        {
            var columnNames = entityPropertyService.GetColumnNamesForType(entityType);
            return entityType.Name switch
            {
                nameof(Establishment) =>
                    SelectFromWhereUrn(columnNames, "v_establishment"),

                nameof(EstablishmentAbsence) =>
                    SelectFromWhereId(columnNames, "v_establishment_absence"),

                nameof(EstablishmentDestinations) =>
                    SelectFromWhereId(columnNames, "v_establishment_destinations"),

                nameof(EstablishmentPerformance) =>
                    SelectFromWhereId(columnNames, "v_establishment_performance"),

                nameof(EstablishmentWorkforce) =>
                    SelectFromWhereId(columnNames, "v_establishment_workforce"),

                nameof(LAAbsence) =>
                    SelectFromWhereId(columnNames, "v_la_absence"),

                nameof(LADestinations) =>
                    SelectFromWhereId(columnNames, "v_la_destinations"),

                nameof(LAPerformance) =>
                    SelectFromWhereId(columnNames, "v_la_performance"),

                nameof(EnglandAbsence) =>
                    SelectFromWhere(columnNames, "v_england_absence", "\"Id\" = 'National'"),

                nameof(EnglandDestinations) =>
                    SelectFromWhere(columnNames, "v_england_destinations", "\"Id\" = 'National'"),

                nameof(EnglandPerformance) =>
                    SelectFromWhere(columnNames, "v_england_performance", "\"Id\" = 'National'"),

                nameof(LaUrls) =>
                    SelectFromWhereId(columnNames, "v_la_urls"),

                nameof(GatewayLocalAuthority) =>
                    SelectFromWhereIdAndNotDeleted(columnNames, "gateway_local_authority"),

                nameof(GatewaySettings) =>
                    SelectFromWhereIdAndNotDeleted(columnNames, "gateway_settings"),

                nameof(GatewayUser) =>
                    SelectFromWhereIdAndNotDeleted(columnNames, "gateway_user"),

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

        public static string GetReadMany(Type entityType, IEntityPropertyService entityPropertyService)
        {
            var columnNames = entityPropertyService.GetColumnNamesForType(entityType);

            return entityType.Name switch
            {
                nameof(Establishment) =>
                    SelectFromWhereUrns(columnNames, "v_establishment"),

                nameof(EstablishmentPerformance) =>
                    SelectFromWhereIds(columnNames, "v_establishment_performance"),

                nameof(EstablishmentSubjectEntryRow) => $"""
                    select
                      {entityPropertyService.GetColumnNamesForType(entityType)}
                    from public.v_establishment_subject_entries
                    where school_urn = @Urn;
                    """,

                nameof(LaUrls) =>
                    SelectFromWhereGSSLACode(columnNames, "v_la_urls"),

                _ => string.Empty,
            };
        }
    }
}
