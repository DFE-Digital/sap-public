-- ================================================================
-- 05_gateway.sql
-- Tables for the gateway service
-- ================================================================

 DROP TABLE IF EXISTS public.gateway_user CASCADE;
 DROP TABLE IF EXISTS public.gateway_user_audit CASCADE;
 DROP TABLE IF EXISTS public.gateway_page_view_audit CASCADE;
 DROP TABLE IF EXISTS public.gateway_local_authority CASCADE;
 DROP TABLE IF EXISTS public.gateway_settings CASCADE;

CREATE TABLE IF NOT EXISTS public.gateway_user
(
    "Id" uuid NOT NULL,
    "EmailAddress" text COLLATE pg_catalog."default",
    "LocalAuthorityId" uuid NOT NULL,
    "CookiePrefs" boolean,
    "TimerStartedOn" timestamp without time zone NOT NULL,
    "CreatedOn" timestamp without time zone,
    "ModifiedOn" timestamp without time zone,
    "AuditIPAddress" text COLLATE pg_catalog."default",
    "IsDeleted" boolean,
    CONSTRAINT gateway_user_pkey PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public.gateway_user_audit
(
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "LoginDateTime" timestamp without time zone,
    "UserAction" text not null,
    "CreatedOn" timestamp without time zone,
    "ModifiedOn" timestamp without time zone,
    "AuditIPAddress" text COLLATE pg_catalog."default",
    "IsDeleted" boolean,
    CONSTRAINT gateway_user_audit_pkey PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public.gateway_local_authority
(
    "Id" uuid NOT NULL,
    "LocalAuthorityName" text COLLATE pg_catalog."default",
    "MaxSessions" integer,
    "CreatedOn" timestamp without time zone,
    "ModifiedOn" timestamp without time zone,
    "AuditIPAddress" text COLLATE pg_catalog."default",
    "IsDeleted" boolean,
    CONSTRAINT gateway_local_authority_pkey PRIMARY KEY ("Id"),
    CONSTRAINT gateway_local_authority_unique_name UNIQUE ("LocalAuthorityName")
);

--- We're using Bury for test and ITHC. 
INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'Bury', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;


INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'LCE', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'ESX', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'BPC', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'CHW', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'HIL', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'WLL', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'NBL', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'BEX', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'NET', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'NWM', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'HRT', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'MRT', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'WIL', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'GRE', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'BDG', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'AFC', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'DNC', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'SFK', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'WAR', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'DAL', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'NSM', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'WSX', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'SGC', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'WNT', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'BIR', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'ERY', 100, now(), now(), '::1', FALSE) ON CONFLICT ("LocalAuthorityName") DO NOTHING;

CREATE TABLE IF NOT EXISTS public.gateway_settings
(
    "Id" uuid NOT NULL,
    "SettingName" text COLLATE pg_catalog."default",
    "SettingValue" text COLLATE pg_catalog."default",
    "CreatedOn" timestamp without time zone,
    "ModifiedOn" timestamp without time zone,
    "AuditIPAddress" text COLLATE pg_catalog."default",
    "IsDeleted" boolean,
    CONSTRAINT gateway_global_settings_pkey PRIMARY KEY ("Id"),
    CONSTRAINT gateway_global_settings_unique_name UNIQUE ("SettingName")
);

INSERT INTO public.gateway_settings(
	"Id", "SettingName", "SettingValue", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
	    VALUES (gen_random_uuid(), 'GlobalEnable', 'true', now(), now(), '::1', FALSE) 
            ON CONFLICT ("SettingName") DO NOTHING;

