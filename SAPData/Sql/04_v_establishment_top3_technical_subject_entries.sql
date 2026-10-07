-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment_top3_technical_subject_entries

DROP MATERIALIZED VIEW IF EXISTS v_establishment_top3_technical_subject_entries;

CREATE MATERIALIZED VIEW v_establishment_top3_technical_subject_entries AS
SELECT
    t."school_urn" AS "URN",
    t."subject_discount_group" AS "SubjectName",
    clean_numeric(t."percentage_entering") AS "PercentageEntering"
FROM t_ks4_top3_technical_s_85f6724725 t
WHERE t."qualification_type" = 'Vocational'
  AND t."time_period" = '202425';

CREATE INDEX idx_v_establishment_top3_technical_subject_entries_urn
    ON v_establishment_top3_technical_subject_entries ("URN");
