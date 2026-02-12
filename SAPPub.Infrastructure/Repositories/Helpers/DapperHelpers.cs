using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.Helpers
{
    public class DapperHelpers
    {
        public static string GetReadMultiple(Type entityName)
        {
            return entityName.Name switch
            {
                nameof(Establishment) => $"select * from v_establishment Where \"IsDeleted\" != true \"PhaseOfEducationId\" = 4;", //We're limiting only to KS4 schools
                nameof(GatewayUser) => $"select * from gateway_user Where \"IsDeleted\" != true ;",
                nameof(GatewayLocalAuthority) => $"select * from gateway_local_authority Where \"IsDeleted\" != true ;",
                nameof(GatewaySettings) => $"select * from gateway_settings Where \"IsDeleted\" != true ;",
                nameof(GatewayPageViewAudit) => $"select * from gateway_page_view_audit Where \"IsDeleted\" != true ;",
                nameof(GatewayUserAudit) => $"select * from gateway_user_audit Where \"IsDeleted\" != true ;",
                _ => $"",
            };
        }

        public static string GetReadSingle(Type entityName)
        {
            return entityName.Name switch
            {
                nameof(Establishment) => $"select * from v_establishment Where \"IsDeleted\" != true AND \"PhaseOfEducationId\" = 4  AND \"URN\" = @Id;", //We're limiting only to KS4 schools
                nameof(GatewayUser) => $"select * from gateway_user Where \"IsDeleted\" != true AND \"Id\" = @Id;",
                nameof(GatewayLocalAuthority) => $"select * from gateway_local_authority Where \"IsDeleted\" != true AND \"Id\" = @Id;",
                nameof(GatewaySettings) => $"select * from gateway_settings Where \"IsDeleted\" != true AND \"Id\" = @Id;",
                nameof(GatewayPageViewAudit) => $"select * from gateway_page_view_audit Where \"IsDeleted\" != true AND \"Id\" = @Id;",
                nameof(GatewayUserAudit) => $"select * from gateway_user_audit Where \"IsDeleted\" != true AND \"Id\" = @Id;",

                _ => $"",
            };
        }

        public static string GetCreate(Type entityName)
        {
            return entityName.Name switch
            {
                nameof(GatewayUser) => $"INSERT INTO \"gateway_user\" (  \"Id\",  \"EmailAddress\",  \"LocalAuthorityId\",  \"CookiePrefs\",  \"RegisteredOn\",  \"CreatedOn\",  \"ModifiedOn\",  \"IsDeleted\") VALUES (  @Id,  @EmailAddress,  @LocalAuthorityId,  @CookiePrefs,  @RegisteredOn,  @CreatedOn,  @ModifiedOn,  @IsDeleted);",
                nameof(GatewayUserAudit) => $"INSERT INTO \"gateway_user_audit\" (  \"Id\",  \"UserId\",  \"LoginDateTime\",  \"CreatedOn\",  \"ModifiedOn\",  \"AuditIPAddress\",  \"IsDeleted\")VALUES (  @Id,  @UserId,  @LoginDateTime,  @CreatedOn,  @ModifiedOn,  @AuditIPAddress,  @IsDeleted);",
                nameof(GatewayPageViewAudit) => $"INSERT INTO \"gateway_page_view_audit\" (  \"Id\",  \"UserId\",  \"URL\",  \"CreatedOn\",  \"ModifiedOn\",  \"AuditIPAddress\",  \"IsDeleted\")VALUES (  @Id,  @UserId,  @URL,  @CreatedOn,  @ModifiedOn,  @AuditIPAddress,  @IsDeleted);",
                nameof(GatewayLocalAuthority) => "INSERT INTO \"gateway_local_authority\" (  \"Id\",  \"LocalAuthorityName\",  \"MaxSessions\",  \"CreatedOn\",  \"ModifiedOn\", \"AuditIPAddress\"  \"IsDeleted\")VALUES (  @Id,  @LocalAuthorityName,  @MaxSessions,  @CreatedOn,  @ModifiedOn, @AuditIPAddress, @IsDeleted);",
                nameof(GatewaySettings) => "INSERT INTO \"gateway_settings\" (  \"Id\",  \"Key\",  \"Value\",  \"CreatedOn\",  \"ModifiedOn\", \"AuditIPAddress\"  \"IsDeleted\")VALUES (  @Id,  @Key,  @Value,  @CreatedOn,  @ModifiedOn, @AuditIPAddress, @IsDeleted);",
                _ => $"",
            };
        }

        public static string GetUpdate(Type entityName)
        {
            return entityName.Name switch
            {
                nameof(GatewayUser) => $"UPDATE gateway_user SET \"EmailAddress\" = @EmailAddress,    \"LocalAuthorityId\" = @LocalAuthorityId,    \"CookiePrefs\" = @CookiePrefs,    \"RegisteredOn\" = @RegisteredOn,    \"CreatedOn\" = @CreatedOn,    \"ModifiedOn\" = @ModifiedOn,    \"AuditIPAddress\" = @AuditIPAddress,    \"IsDeleted\" = @IsDeleted WHERE \"Id\" = @Id;",
                nameof(GatewayUserAudit) => $"UPDATE gateway_user_audit SET \"UserId\"=@UserId, \"LoginDateTime\"=@LoginDateTime, \"CreatedOn\"=@CreatedOn, \"ModifiedOn\"=@ModifiedOn, \"AuditIPAddress\"=@AuditIPAddress, \"IsDeleted\"=@IsDeleted WHERE \"Id\"=@Id;",
                nameof(GatewayPageViewAudit) => $"UPDATE gateway_page_view_audit SET \"UserId\"=@UserId, \"URL\"=@URL, \"CreatedOn\"=@CreatedOn, \"ModifiedOn\"=@ModifiedOn, \"AuditIPAddress\"=@AuditIPAddress, \"IsDeleted\"=@IsDeleted WHERE \"Id\"=@Id;",
                nameof(GatewayLocalAuthority) => "UPDATE gateway_local_authority SET \"LocalAuthorityName\"=@LocalAuthorityName, \"MaxSessions\"=@MaxSessions, \"CreatedOn\"=@CreatedOn, \"ModifiedOn\"=@ModifiedOn, \"AuditIPAddress\"=@AuditIPAddress, \"IsDeleted\"=@IsDeleted WHERE \"Id\"=@Id;",
                nameof(GatewaySettings) => "UPDATE gateway_settings SET \"SettingName\"=@SettingName, \"SettingValue\"=@SettingValue, \"CreatedOn\"=@CreatedOn, \"ModifiedOn\"=@ModifiedOn, \"AuditIPAddress\"=@AuditIPAddress, \"IsDeleted\"=@IsDeleted WHERE \"Id\"=@Id;",
                _ => $"",
            };
        }
    }
}
