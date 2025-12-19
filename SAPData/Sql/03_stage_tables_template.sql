-- ================================================================
-- 03_stage_tables.sql
-- Generated from template – DO NOT EDIT
-- ================================================================

-- ================================================================
-- SAFE CONVERTERS
-- ================================================================

CREATE OR REPLACE FUNCTION clean_int(value TEXT) RETURNS INT AS $$
BEGIN
    IF value IS NULL OR trim(value) IN ('', 'NE','N','na','n/a','N/A','SUPP','.','-','--') THEN
        RETURN NULL;
    END IF;
    RETURN value::INT;
EXCEPTION WHEN others THEN
    RETURN NULL;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

CREATE OR REPLACE FUNCTION clean_numeric(value TEXT) RETURNS NUMERIC AS $$
BEGIN
    IF value IS NULL OR trim(value) IN ('', 'NE','N','na','n/a','N/A','SUPP','.','-','--') THEN
        RETURN NULL;
    END IF;
    RETURN value::NUMERIC;
EXCEPTION WHEN others THEN
    RETURN NULL;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

CREATE OR REPLACE FUNCTION clean_date(value TEXT) RETURNS DATE AS $$
BEGIN
    IF value IS NULL OR trim(value) IN ('', 'na','n/a','N/A','.','-','--') THEN
        RETURN NULL;
    END IF;
    RETURN value::DATE;
EXCEPTION WHEN others THEN
    RETURN NULL;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

-- ================================================================
-- GIAS
-- ================================================================

DROP TABLE IF EXISTS stg_gias;

CREATE TABLE stg_gias AS
SELECT
    clean_int("URN")                  AS urn,
    clean_int("LA (code)")             AS la_code,
    "LA (name)"                       AS la_name,
    "EstablishmentName"               AS school_name,
    "PhaseOfEducation (name)"         AS phase,
    "AdmissionsPolicy (name)"         AS admissions_policy,
    "TypeOfEstablishment (name)"      AS establishment_type,
    "EstablishmentTypeGroup (name)"   AS establishment_group,
    "Trusts (name)"                   AS trust_name,
    "GOR (name)"                      AS region,
    "Postcode"                        AS postcode
FROM {{GIAS}};

-- ================================================================
-- KS4 DESTINATIONS – INSTITUTION
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_dest_inst;

CREATE TABLE stg_ks4_dest_inst AS
SELECT
    clean_int(time_period)     AS time_period,
    time_identifier,
    NULL::INT                  AS urn,
    school_laestab,
    clean_numeric(overall)     AS overall,
    clean_numeric(education)   AS education,
    clean_numeric(all_work)    AS employment,
    clean_numeric(appren)      AS apprentice
FROM {{KS4_DEST_INST_2021}}

UNION ALL

SELECT
    clean_int(time_period),
    time_identifier,
    clean_int(school_urn),
    school_laestab,
    clean_numeric(overall),
    clean_numeric(education),
    clean_numeric(all_work),
    clean_numeric(appren)
FROM {{KS4_DEST_INST_2022}};

-- ================================================================
-- KS4 DESTINATIONS – LA
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_dest_la;

CREATE TABLE stg_ks4_dest_la AS
SELECT
    clean_int(new_la_code)     AS la_code,
    clean_int(time_period)     AS time_period,
    time_identifier,
    clean_numeric(overall)     AS overall,
    clean_numeric(education)   AS education,
    clean_numeric(all_work)    AS all_work,
    clean_numeric(appren)      AS apprentice
FROM {{KS4_DEST_LA_2021}}

UNION ALL

SELECT
    clean_int(lad_code),
    clean_int(time_period),
    time_identifier,
    clean_numeric(overall),
    clean_numeric(education),
    clean_numeric(all_work),
    clean_numeric(appren)
FROM {{KS4_DEST_LA_2022}};

-- ================================================================
-- KS4 DESTINATIONS – ENGLAND
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_dest_eng;

CREATE TABLE stg_ks4_dest_eng AS
SELECT
    clean_int(time_period)     AS time_period,
    time_identifier,
    clean_numeric(overall)     AS overall,
    clean_numeric(education)   AS education,
    clean_numeric(all_work)    AS all_work,
    clean_numeric(appren)      AS apprentice
FROM {{KS4_DEST_ENG_2021}}

UNION ALL

SELECT
    clean_int(time_period),
    time_identifier,
    clean_numeric(overall),
    clean_numeric(education),
    clean_numeric(all_work),
    clean_numeric(appren)
FROM {{KS4_DEST_ENG_2022}};

-- ================================================================
-- KS4 PERFORMANCE – INSTITUTION
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_perf_inst;

CREATE TABLE stg_ks4_perf_inst AS
SELECT
    202223                      AS time_period,
    'Current'                   AS time_identifier,
    clean_int("URN")            AS urn,
    clean_numeric("ATT8SCR")    AS att8,
    clean_numeric("P8MEA")      AS p8
FROM {{KS4_PERF_INST_2022}}

UNION ALL

SELECT
    clean_int(time_period),
    time_identifier,
    clean_int(school_urn),
    clean_numeric(avg_att8),
    clean_numeric(avg_p8score)
FROM {{KS4_PERF_INST_2023}}

UNION ALL

SELECT
    clean_int(time_period),
    time_identifier,
    clean_int(school_urn),
    clean_numeric(attainment8_average),
    clean_numeric(progress8_average)
FROM {{KS4_PERF_INST_2024}};

-- ================================================================
-- KS4 PERFORMANCE – LA
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_perf_la;

CREATE TABLE stg_ks4_perf_la AS
SELECT
    clean_int(new_la_code)      AS la_code,
    clean_int(time_period)  AS time_period,
    time_identifier,
    clean_numeric(att8scr)  AS att8scr,
    clean_numeric(p8mea)    AS p8
FROM {{KS4_PERF_LA}};

-- ================================================================
-- KS4 PERFORMANCE – ENGLAND
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_perf_eng;

CREATE TABLE stg_ks4_perf_eng AS
SELECT
    clean_int(time_period)  AS time_period,
    time_identifier,
    clean_numeric(att8scr)  AS att8scr,
    clean_numeric(p8mea)    AS p8
FROM {{KS4_PERF_ENG}};
