-- ================================================================
-- 04_indexes.sql
-- Auto-generated indexes for analytical views
-- ================================================================

CREATE INDEX IF NOT EXISTS idx_v_england_destinations_time_period
ON v_england_destinations (time_period);

CREATE INDEX IF NOT EXISTS idx_v_england_performance_time_period
ON v_england_performance (time_period);

CREATE INDEX IF NOT EXISTS idx_v_establishment_urn
ON v_establishment (urn);

CREATE INDEX IF NOT EXISTS idx_v_establishment_absence_urn
ON v_establishment_absence (urn);

CREATE INDEX IF NOT EXISTS idx_v_establishment_destinations_urn
ON v_establishment_destinations (urn);

CREATE INDEX IF NOT EXISTS idx_v_establishment_performance_urn
ON v_establishment_performance (urn);

CREATE INDEX IF NOT EXISTS idx_v_establishment_workforce_urn
ON v_establishment_workforce (urn);

CREATE INDEX IF NOT EXISTS idx_v_la_destinations_la_code
ON v_la_destinations (la_code);

CREATE INDEX IF NOT EXISTS idx_v_la_performance_la_code
ON v_la_performance (la_code);

