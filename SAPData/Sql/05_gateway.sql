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
    "RegisteredOn" timestamp without time zone,
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
    "CreatedOn" timestamp without time zone,
    "ModifiedOn" timestamp without time zone,
    "AuditIPAddress" text COLLATE pg_catalog."default",
    "IsDeleted" boolean,
    CONSTRAINT gateway_user_audit_pkey PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public.gateway_page_view_audit
(
    "Id" uuid NOT NULL,
    "UserId" uuid NULL,
    "URL" text COLLATE pg_catalog."default",
    "CreatedOn" timestamp without time zone,
    "ModifiedOn" timestamp without time zone,
    "AuditIPAddress" text COLLATE pg_catalog."default",
    "IsDeleted" boolean,
    CONSTRAINT gateway_page_view_audit_pkey PRIMARY KEY ("Id")
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

INSERT INTO public.gateway_local_authority(
	"Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
	VALUES (gen_random_uuid(), 'Bury', 100, now(), now(), '::1', FALSE);
INSERT INTO public.gateway_local_authority(
	"Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
    VALUES (gen_random_uuid(), 'Bolton', 100, now(), now(), '::1', FALSE);
INSERT INTO public.gateway_local_authority(
	"Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
    VALUES (gen_random_uuid(), 'Manchester City Council', 100, now(), now(), '::1', FALSE);

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
	VALUES (gen_random_uuid(), 'GlobalEnable', 'true', now(), now(), '::1', FALSE);

