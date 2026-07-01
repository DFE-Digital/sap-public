-- ================================================================
-- run_all.sql
-- ================================================================

\set ON_ERROR_STOP on
\set AUTOCOMMIT off

BEGIN;

\ir 00_cleanup.sql
\ir 01_reference_tables.sql
\ir 02_create_raw_tables.sql
\ir 03_copy_into_raw_local.sql
\ir 04_v_england_absence.sql
\ir 04_v_england_destinations.sql
\ir 04_v_england_performance.sql
\ir 04_v_england_ks5_destinations.sql
\ir 04_v_england_ks5_performance.sql
\ir 04_v_england_ks2_attainment.sql
\ir 04_v_establishment.sql
\ir 04_v_establishment_links.sql
\ir 04_v_establishment_group_links.sql
\ir 04_v_establishment_absence.sql
\ir 04_v_establishment_destinations.sql
\ir 04_v_establishment_performance.sql
\ir 04_v_establishment_workforce.sql
\ir 04_v_establishment_ks5_destinations.sql
\ir 04_v_establishment_ks5_performance.sql
\ir 04_v_establishment_ks2_attainment.sql
\ir 04_v_establishment_subject_entries.sql
\ir 04_v_la_absence.sql
\ir 04_v_la_destinations.sql
\ir 04_v_la_performance.sql
\ir 04_v_la_ks5_destinations.sql
\ir 04_v_la_ks5_performance.sql
\ir 04_v_la_ks2_attainment.sql
\ir 04_v_la_subject_entries.sql
\ir 04_v_la_urls.sql
\ir 05_indexes.sql
\ir 06_gateway.sql
--\ir 05_validation.sql

COMMIT;





