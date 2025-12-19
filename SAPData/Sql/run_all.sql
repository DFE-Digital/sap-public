-- ================================================================
-- run_all.sql
-- ================================================================

\set ON_ERROR_STOP on

\i 00_cleanup.sql
\i 01_create_raw_tables.sql
\i 02_copy_into_raw.sql
\i 03_v_england_destinations.sql
\i 03_v_england_performance.sql
\i 03_v_establishment.sql
\i 03_v_establishment_absence.sql
\i 03_v_establishment_destinations.sql
\i 03_v_establishment_performance.sql
\i 03_v_establishment_workforce.sql
\i 03_v_la_destinations.sql
\i 03_v_la_performance.sql
\i 04_indexes.sql
--\i 05_validation.sql







