--- We're using Bury for test and ITHC. 

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'Bury', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'BuryOne', 1, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_user(
	"Id", "EmailAddress", "LocalAuthorityId", "CookiePrefs", "TimerStartedOn", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'test@example.org', (select "Id" from public.gateway_local_authority where "LocalAuthorityName" = 'Bury'), 
	'false', '2026-03-01 23:39:58.879496', '2026-03-01 23:39:58.879496', '2026-03-01 23:39:58.879496', '::1', false);