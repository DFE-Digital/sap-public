-- ================================================================
-- 03_stage_tables.sql
-- Staging layer: cleaned / normalised projections of raw_* tables
-- ================================================================

\echo 'Creating staging tables...'

-- ----------------------------------------------------------------
-- UNIVERSAL SAFE CONVERTERS
-- ----------------------------------------------------------------

CREATE OR REPLACE FUNCTION clean_int(value TEXT) RETURNS INT AS $$
BEGIN
    IF value IS NULL OR btrim(value) IN ('', 'NE','N','na','n/a','N/A','SUPP','.','-','--') THEN 
        RETURN NULL;
    END IF;
    RETURN value::INT;
EXCEPTION WHEN others THEN
    RETURN NULL;
END; $$ LANGUAGE plpgsql IMMUTABLE;

CREATE OR REPLACE FUNCTION clean_numeric(value TEXT) RETURNS NUMERIC AS $$
BEGIN
    IF value IS NULL OR btrim(value) IN ('', 'NE','N','na','n/a','N/A','SUPP','.','-','--') THEN 
        RETURN NULL;
    END IF;
    RETURN value::NUMERIC;
EXCEPTION WHEN others THEN
    RETURN NULL;
END; $$ LANGUAGE plpgsql IMMUTABLE;

CREATE OR REPLACE FUNCTION clean_date(value TEXT) RETURNS DATE AS $$
BEGIN
    IF value IS NULL OR btrim(value) IN ('', 'na','n/a','N/A','.','-','--') THEN 
        RETURN NULL;
    END IF;
    RETURN value::DATE;
EXCEPTION WHEN others THEN
    RETURN NULL;
END; $$ LANGUAGE plpgsql IMMUTABLE;

-- ================================================================
-- 1. GIAS → stg_gias
-- ================================================================

DROP TABLE IF EXISTS stg_gias;

CREATE TABLE stg_gias AS
SELECT
    clean_int("URN") AS urn,
    clean_int("LA (code)") AS la_code,
    "LA (name)" AS la_name,
    clean_int("EstablishmentNumber") AS establishment_number,

    "EstablishmentName" AS school_name,
    "TypeOfEstablishment (name)" AS establishment_type,
    "EstablishmentTypeGroup (name)" AS establishment_group,
    "AdmissionsPolicy (name)" AS admissions_policy,
    "PhaseOfEducation (name)" AS phase,

    "Trusts (code)" AS trust_code,
    "Trusts (name)" AS trust_name,
    "Federations (name)" AS federation,

    "HeadTitle (name)" AS head_title,
    "HeadFirstName" AS head_first_name,
    "HeadLastName" AS head_last_name,
    "HeadPreferredJobTitle" AS head_job_title,

    "Gender (name)" AS gender,
    clean_int("StatutoryLowAge") AS statutory_low_age,
    clean_int("StatutoryHighAge") AS statutory_high_age,

    "Street" AS street,
    "Locality" AS locality,
    "Address3" AS address3,
    "Town" AS town,
    "County (name)" AS county,
    "Postcode" AS postcode,
    "GOR (name)" AS region,
    "DistrictAdministrative (name)" AS district,
    "AdministrativeWard (name)" AS ward,
    "ParliamentaryConstituency (name)" AS constituency,
    "UrbanRural (name)" AS urban_rural,

    "SchoolWebsite" AS website,
    "TelephoneNum" AS telephone,

    "EstablishmentStatus (name)" AS establishment_status,
    clean_date("OpenDate") AS open_date,
    clean_date("CloseDate") AS close_date,
    clean_date("LastChangedDate") AS last_changed_date,
    clean_date("DateOfLastInspectionVisit") AS last_inspection_date,
    clean_date("NextInspectionVisit") AS next_inspection_date,

    "ReligiousCharacter (name)" AS religious_character,
    "ReligiousEthos (name)" AS religious_ethos,
    "Diocese (name)" AS diocese,

    "BSOInspectorateName (name)" AS inspectorate_name,
    "InspectorateReport" AS inspectorate_report,

    clean_int("SchoolCapacity") AS school_capacity,
    clean_int("NumberOfPupils") AS number_of_pupils,
    clean_int("NumberOfBoys") AS number_of_boys,
    clean_int("NumberOfGirls") AS number_of_girls,
    clean_numeric("PercentageFSM") AS percentage_fsm,

    clean_int("Easting") AS easting,
    clean_int("Northing") AS northing,
    "MSOA (name)" AS msoa_name,
    "LSOA (name)" AS lsoa_name,
    "MSOA (code)" AS msoa_code,
    "LSOA (code)" AS lsoa_code,

    "UKPRN" AS ukprn,
    "FEHEIdentifier" AS fehe_identifier,
    "FurtherEducationType (name)" AS fe_type,

    "UPRN" AS uprn,
    "SiteName" AS site_name,
    "PropsName" AS props_name,
    "Country (name)" AS country,
    "FSM" AS fsm_raw,

    clean_date("AccreditationExpiryDate") AS accreditation_expiry_date

FROM raw_gias_95e03347;

-- ================================================================
-- 2. KS4 DESTINATIONS — Institution Level (2021/22 + 2022/23)
--    2021/22 has no URN column: we keep school_laestab for traceability.
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_dest_inst;

CREATE TABLE stg_ks4_dest_inst AS
SELECT
    clean_int(time_period)                  AS time_period,
    time_identifier                         AS time_identifier,
    NULL::INT                               AS urn,
    school_laestab                          AS school_laestab,
    clean_numeric(overall)                  AS overall,
    clean_numeric(education)                AS education,
    clean_numeric(all_work)                 AS employment,
    clean_numeric(appren)                   AS apprentice
FROM raw_ks4_dm_ud_202122_ins_719232cd

UNION ALL

SELECT
    clean_int(time_period)                  AS time_period,
    time_identifier                         AS time_identifier,
    clean_int(school_urn)                   AS urn,
    school_laestab                          AS school_laestab,
    clean_numeric(overall)                  AS overall,
    clean_numeric(education)                AS education,
    clean_numeric(all_work)                 AS employment,
    clean_numeric(appren)                   AS apprentice
FROM raw_ks4_dm_ud_202223_ins_fed8f8f8;

-- ================================================================
-- 3. KS4 DESTINATIONS — LA Level (2021/22 + 2022/23)
--    2021/22 uses new_la_code; 2022/23 uses lad_code
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_dest_la;

CREATE TABLE stg_ks4_dest_la AS
SELECT
    clean_int(new_la_code)                  AS la_code,
    clean_int(time_period)                  AS time_period,
    time_identifier                         AS time_identifier,
    clean_numeric(overall)                  AS overall,
    clean_numeric(education)                AS education,
    clean_numeric(all_work)                 AS employment,
    clean_numeric(appren)                   AS apprentice
FROM raw_ks4_dm_ud_202122_la__6ddc05df

UNION ALL

SELECT
    clean_int(lad_code)                     AS la_code,
    clean_int(time_period)                  AS time_period,
    time_identifier                         AS time_identifier,
    clean_numeric(overall)                  AS overall,
    clean_numeric(education)                AS education,
    clean_numeric(all_work)                 AS employment,
    clean_numeric(appren)                   AS apprentice
FROM raw_ks4_dm_ud_202223_la__fa9dccf0;

-- ================================================================
-- 4. KS4 DESTINATIONS — England Level (2021/22 + 2022/23)
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_dest_eng;

CREATE TABLE stg_ks4_dest_eng AS
SELECT
    clean_int(time_period)               AS time_period,
    time_identifier                      AS time_identifier,
    clean_numeric(overall)               AS overall,
    clean_numeric(education)             AS education,
    clean_numeric(all_work)              AS employment,
    clean_numeric(appren)                AS apprentice
FROM raw_ks4_dm_ud_202122_nat_f21db4bf

UNION ALL

SELECT
    clean_int(time_period),
    time_identifier,
    clean_numeric(overall),
    clean_numeric(education),
    clean_numeric(all_work),
    clean_numeric(appren)
FROM raw_ks4_dm_ud_202223_nat_91f39145;

-- ================================================================
-- 5. KS4 PERFORMANCE — Institution Level (2022/23 + 2023/24 + 2024/25)
--    We stage a common shape for downstream facts/views.
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_perf_inst;

CREATE TABLE stg_ks4_perf_inst AS
-- 5.1 2022/23 (fixed-width style CSV, header is UPPERCASE)
SELECT
    202223                                        AS time_period,
    'Current'::TEXT                               AS time_identifier,
    NULL::TEXT                                    AS school_laestab,
    clean_int("URN")                              AS urn,
    clean_numeric("ATT8SCR")                      AS att8,
    clean_numeric("P8MEA")                        AS p8,
    clean_numeric("PTL2BASICS_95")                AS engmaths_9,
    clean_numeric("PTL2BASICS_94")                AS engmaths_5,
    NULL::NUMERIC                                 AS ppe,
    NULL::NUMERIC                                 AS ppe2
FROM raw_2022_2023_england_ks_52139976

UNION ALL

-- 5.2 2023/24 (final)
SELECT
    clean_int(time_period)                        AS time_period,
    time_identifier                               AS time_identifier,
    school_laestab                                AS school_laestab,
    clean_int(school_urn)                         AS urn,
    clean_numeric(avg_att8)                       AS att8,
    clean_numeric(avg_p8score)                    AS p8,
    clean_numeric(pt_l2basics_95)                 AS engmaths_9,
    clean_numeric(pt_l2basics_94)                 AS engmaths_5,
    NULL::NUMERIC                                 AS ppe,
    NULL::NUMERIC                                 AS ppe2
FROM raw_202324_performance_t_34a58878
WHERE breakdown IS NULL OR breakdown = '' OR lower(breakdown) IN ('all pupils','total')

UNION ALL

-- 5.3 2024/25 (provisional)
SELECT
    clean_int(time_period)                        AS time_period,
    time_identifier                               AS time_identifier,
    school_laestab                                AS school_laestab,
    clean_int(school_urn)                         AS urn,
    clean_numeric(attainment8_average)            AS att8,
    clean_numeric(progress8_average)              AS p8,
    clean_numeric(engmath_95_percent)             AS engmaths_9,
    clean_numeric(engmath_94_percent)             AS engmaths_5,
    NULL::NUMERIC                                 AS ppe,
    NULL::NUMERIC                                 AS ppe2
FROM raw_202425_performance_t_4962df0d
WHERE breakdown IS NULL OR breakdown = '' OR lower(breakdown) IN ('all pupils','total');

-- ================================================================
-- 6. KS4 PERFORMANCE — LA Level
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_perf_la;

CREATE TABLE stg_ks4_perf_la AS
SELECT
    clean_int(la_code)             AS la_code,
    clean_int(time_period)         AS time_period,
    time_identifier,
    clean_numeric(att8scr)         AS att8,
    clean_numeric(p8mea)           AS p8
FROM raw_ees_ks4_la_202223_e7962c68;

-- ================================================================
-- 7. KS4 PERFORMANCE — England Level
-- ================================================================

DROP TABLE IF EXISTS stg_ks4_perf_eng;

CREATE TABLE stg_ks4_perf_eng AS
SELECT
    clean_int(time_period)         AS time_period,
    time_identifier,
    clean_numeric(att8scr)         AS att8,
    clean_numeric(p8mea)           AS p8
FROM raw_ees_ks4_nat_202223_3ded4415;

-- ================================================================
-- 8. PASS-THROUGH STAGING FOR OTHER DATASETS (keeps ingestion simple)
--    These tables are typed as TEXT in RAW; staging is currently a 1:1 copy.
-- ================================================================

DO $$
BEGIN
    -- Absence
    EXECUTE 'DROP TABLE IF EXISTS stg_1a_absence_3term_school';
    EXECUTE 'CREATE TABLE stg_1a_absence_3term_school AS SELECT * FROM raw_1a_absence_3term_sch_ca32c07a';

    EXECUTE 'DROP TABLE IF EXISTS stg_1_absence_3term_nat_reg_la';
    EXECUTE 'CREATE TABLE stg_1_absence_3term_nat_reg_la AS SELECT * FROM raw_1_absence_3term_nat__098f4c6d';

    -- Exclusions
    EXECUTE 'DROP TABLE IF EXISTS stg_exc_school';
    EXECUTE 'CREATE TABLE stg_exc_school AS SELECT * FROM raw_exc_school_7f81068e';

    EXECUTE 'DROP TABLE IF EXISTS stg_exc_nat_region_la';
    EXECUTE 'CREATE TABLE stg_exc_nat_region_la AS SELECT * FROM raw_exc_nat_region_la_8ee0b1f1';

    -- Workforce
    EXECUTE 'DROP TABLE IF EXISTS stg_workforce_ptrs_2010_2024_sch';
    EXECUTE 'CREATE TABLE stg_workforce_ptrs_2010_2024_sch AS SELECT * FROM raw_workforce_ptrs_2010__9ddcb364';

    -- Characteristics / subjects (2022/23, 2023/24, 2024/25)
    EXECUTE 'DROP TABLE IF EXISTS stg_2223_la_char_data_revised';
    EXECUTE 'CREATE TABLE stg_2223_la_char_data_revised AS SELECT * FROM raw_2223_la_char_data_re_b7cbaa33';

    EXECUTE 'DROP TABLE IF EXISTS stg_202324_la_char_data_revised';
    EXECUTE 'CREATE TABLE stg_202324_la_char_data_revised AS SELECT * FROM raw_202324_la_char_data__880774fa';

    EXECUTE 'DROP TABLE IF EXISTS stg_202425_all_state_funded_pupils_characteristics_and_geography_breakdowns_provisional';
    EXECUTE 'CREATE TABLE stg_202425_all_state_funded_pupils_characteristics_and_geography_breakdowns_provisional AS SELECT * FROM raw_202425_all_state_fun_c05ebf35';

    EXECUTE 'DROP TABLE IF EXISTS stg_2223_subject_pupil_level_la_data_revised';
    EXECUTE 'CREATE TABLE stg_2223_subject_pupil_level_la_data_revised AS SELECT * FROM raw_2223_subject_pupil_l_35b7add0';

    EXECUTE 'DROP TABLE IF EXISTS stg_202324_subject_pupil_level_la_data_revised';
    EXECUTE 'CREATE TABLE stg_202324_subject_pupil_level_la_data_revised AS SELECT * FROM raw_202324_subject_pupil_dcdbea70';

    EXECUTE 'DROP TABLE IF EXISTS stg_202425_subject_local_authority_pupil_entriesachievements_provisional';
    EXECUTE 'CREATE TABLE stg_202425_subject_local_authority_pupil_entriesachievements_provisional AS SELECT * FROM raw_202425_subject_local_1806fcf8';

    EXECUTE 'DROP TABLE IF EXISTS stg_202324_subject_school_all_exam_entriesgrades_final';
    EXECUTE 'CREATE TABLE stg_202324_subject_school_all_exam_entriesgrades_final AS SELECT * FROM raw_202324_subject_schoo_bb2b11ed';

    EXECUTE 'DROP TABLE IF EXISTS stg_202425_subject_school_all_exam_entriesgrades_provisional';
    EXECUTE 'CREATE TABLE stg_202425_subject_school_all_exam_entriesgrades_provisional AS SELECT * FROM raw_202425_subject_schoo_ef61b2cc';

    EXECUTE 'DROP TABLE IF EXISTS stg_england_ks4underlying_entriesandgrades_2';
    EXECUTE 'CREATE TABLE stg_england_ks4underlying_entriesandgrades_2 AS SELECT * FROM raw_england_ks4underlyin_e2ccd47f';

    -- EES KS4 inst
    EXECUTE 'DROP TABLE IF EXISTS stg_ees_ks4_inst_202223';
    EXECUTE 'CREATE TABLE stg_ees_ks4_inst_202223 AS SELECT * FROM raw_ees_ks4_inst_202223_2aabf125';

    -- Edubase snapshot
    EXECUTE 'DROP TABLE IF EXISTS stg_edubasealldata20251022';
    EXECUTE 'CREATE TABLE stg_edubasealldata20251022 AS SELECT * FROM raw_edubasealldata202510_583572b5';
END $$;

\echo 'Staging complete.'
