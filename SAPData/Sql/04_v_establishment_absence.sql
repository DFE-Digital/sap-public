-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment_absence

DROP MATERIALIZED VIEW IF EXISTS v_establishment_absence;

CREATE MATERIALIZED VIEW v_establishment_absence AS
WITH
src_1 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."time_period" = '202425' AND t."education_phase" = 'State-funded primary' THEN t."sess_overall_percent"::text END) AS "Abs_TotKS2_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."education_phase" = 'State-funded secondary' THEN t."sess_overall_percent"::text END) AS "Abs_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."education_phase" = 'Special' THEN t."sess_overall_percent"::text END) AS "Abs_TotSPE_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."education_phase" = 'State-funded primary' THEN t."enrolments_pa_10_exact_percent"::text END) AS "Abs_PersistentKS2_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."education_phase" = 'State-funded secondary' THEN t."enrolments_pa_10_exact_percent"::text END) AS "Abs_Persistent_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."education_phase" = 'Special' THEN t."enrolments_pa_10_exact_percent"::text END) AS "Abs_PersistentSPE_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202324' AND t."education_phase" = 'State-funded primary' THEN t."enrolments"::text END) AS "Enrolments_TotKS2_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202324' AND t."education_phase" = 'State-funded secondary' THEN t."enrolments"::text END) AS "Enrolments_Tot_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202324' AND t."education_phase" = 'Special' THEN t."enrolments"::text END) AS "Enrolments_TotSPE_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202324' AND t."education_phase" = 'State-funded primary' THEN t."enrolments_pa_10_exact"::text END) AS "Abs_PersistentKS2_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202324' AND t."education_phase" = 'State-funded secondary' THEN t."enrolments_pa_10_exact"::text END) AS "Abs_Persistent_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202324' AND t."education_phase" = 'Special' THEN t."enrolments_pa_10_exact"::text END) AS "Abs_PersistentSPE_Est_Current_Num_Coded"
    FROM t_1a_absence_3term_sch_d1b51341e3 t
    GROUP BY t."school_urn"
)
,
all_ids AS (
    SELECT "Id" FROM src_1
)

SELECT
    a."Id" AS "Id",
    e."LAId" AS "LAId",
    e."LAName" AS "LAName",
    e."RegionId" AS "RegionId",
    e."RegionName" AS "RegionName",
    e."LAId" || e."EstablishmentNumber" AS "LAEstab",
    src_1."Abs_Persistent_Est_Current_Num_Coded" AS "Abs_Persistent_Est_Current_Num_Coded",
    src_1."Abs_Persistent_Est_Current_Pct_Coded" AS "Abs_Persistent_Est_Current_Pct_Coded",
    src_1."Abs_PersistentKS2_Est_Current_Num_Coded" AS "Abs_PersistentKS2_Est_Current_Num_Coded",
    src_1."Abs_PersistentKS2_Est_Current_Pct_Coded" AS "Abs_PersistentKS2_Est_Current_Pct_Coded",
    src_1."Abs_PersistentSPE_Est_Current_Num_Coded" AS "Abs_PersistentSPE_Est_Current_Num_Coded",
    src_1."Abs_PersistentSPE_Est_Current_Pct_Coded" AS "Abs_PersistentSPE_Est_Current_Pct_Coded",
    src_1."Abs_Tot_Est_Current_Pct_Coded" AS "Abs_Tot_Est_Current_Pct_Coded",
    src_1."Abs_TotKS2_Est_Current_Pct_Coded" AS "Abs_TotKS2_Est_Current_Pct_Coded",
    src_1."Abs_TotSPE_Est_Current_Pct_Coded" AS "Abs_TotSPE_Est_Current_Pct_Coded",
    src_1."Enrolments_Tot_Est_Current_Num_Coded" AS "Enrolments_Tot_Est_Current_Num_Coded",
    src_1."Enrolments_TotKS2_Est_Current_Num_Coded" AS "Enrolments_TotKS2_Est_Current_Num_Coded",
    src_1."Enrolments_TotSPE_Est_Current_Num_Coded" AS "Enrolments_TotSPE_Est_Current_Num_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
LEFT JOIN v_establishment e ON e."URN" = a."Id"
;
