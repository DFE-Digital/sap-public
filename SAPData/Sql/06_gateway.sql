-- ================================================================
-- 06_gateway.sql
-- Tables for the gateway service
-- ================================================================

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
    "UserAction" text NOT NULL,
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

-- Ensure unique constraint exists when table already existed before this script
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'gateway_local_authority_unique_name'
          AND conrelid = 'public.gateway_local_authority'::regclass
    ) THEN
        ALTER TABLE public.gateway_local_authority
            ADD CONSTRAINT gateway_local_authority_unique_name UNIQUE ("LocalAuthorityName");
    END IF;
END $$;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'Bury', 100, now(), now(), '::1', FALSE)
ON CONFLICT ("LocalAuthorityName") DO NOTHING;

INSERT INTO public.gateway_local_authority("Id", "LocalAuthorityName", "MaxSessions", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'Bolton', 100, now(), now(), '::1', FALSE)
ON CONFLICT ("LocalAuthorityName") DO NOTHING;

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

-- Ensure unique constraint exists when table already existed before this script
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'gateway_global_settings_unique_name'
          AND conrelid = 'public.gateway_settings'::regclass
    ) THEN
        ALTER TABLE public.gateway_settings
            ADD CONSTRAINT gateway_global_settings_unique_name UNIQUE ("SettingName");
    END IF;
END $$;

INSERT INTO public.gateway_settings(
    "Id", "SettingName", "SettingValue", "CreatedOn", "ModifiedOn", "AuditIPAddress", "IsDeleted")
VALUES (gen_random_uuid(), 'GlobalEnable', 'true', now(), now(), '::1', FALSE)
ON CONFLICT ("SettingName") DO NOTHING;