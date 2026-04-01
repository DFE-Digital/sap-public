--- We're using Bury for test and ITHC. 
INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'Bury', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;