-- AUTO-GENERATED MATERIALIZED VIEW: v_england_absence

DROP MATERIALIZED VIEW IF EXISTS v_england_absence;

CREATE MATERIALIZED VIEW v_england_absence AS
WITH
src_1 AS (
    SELECT
        t."geographic_level" AS "Id",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202425' AND t."education_phase" = 'State-funded primary' THEN t."sess_overall_percent"::text END) AS "Abs_TotKS2_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202425' AND t."education_phase" = 'State-funded secondary' THEN t."sess_overall_percent"::text END) AS "Abs_Tot_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202425' AND t."education_phase" = 'Special' THEN t."sess_overall_percent"::text END) AS "Abs_TotSPE_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202425' AND t."education_phase" = 'State-funded primary' THEN t."enrolments_pa_10_exact_percent"::text END) AS "Abs_PersistentKS2_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202425' AND t."education_phase" = 'State-funded secondary' THEN t."enrolments_pa_10_exact_percent"::text END) AS "Abs_Persistent_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202425' AND t."education_phase" = 'Special' THEN t."enrolments_pa_10_exact_percent"::text END) AS "Abs_PersistentSPE_Eng_Current_Pct_Coded"
    FROM t_1_absence_3term_nat__2642eb995e t
    GROUP BY t."geographic_level"
)
,
all_ids AS (
    SELECT "Id" FROM src_1
)

SELECT
    a."Id" AS "Id",
    src_1."Abs_Persistent_Eng_Current_Pct_Coded" AS "Abs_Persistent_Eng_Current_Pct_Coded",
    src_1."Abs_PersistentKS2_Eng_Current_Pct_Coded" AS "Abs_PersistentKS2_Eng_Current_Pct_Coded",
    src_1."Abs_PersistentSPE_Eng_Current_Pct_Coded" AS "Abs_PersistentSPE_Eng_Current_Pct_Coded",
    src_1."Abs_Tot_Eng_Current_Pct_Coded" AS "Abs_Tot_Eng_Current_Pct_Coded",
    src_1."Abs_TotKS2_Eng_Current_Pct_Coded" AS "Abs_TotKS2_Eng_Current_Pct_Coded",
    src_1."Abs_TotSPE_Eng_Current_Pct_Coded" AS "Abs_TotSPE_Eng_Current_Pct_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
;
