-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment_destinations

DROP MATERIALIZED VIEW IF EXISTS v_establishment_destinations;

CREATE MATERIALIZED VIEW v_establishment_destinations AS
WITH
src_1 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."appren"::text END) AS "Apprentice_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."appren"::text END) AS "Apprentice_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."cohort"::text END) AS "Cohort_Tot_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."cohort"::text END) AS "Cohort_Dis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."education"::text END) AS "Education_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."education"::text END) AS "Education_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."all_work"::text END) AS "Employment_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."all_work"::text END) AS "Employment_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."fe"::text END) AS "FurtherEd_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."fe"::text END) AS "FurtherEd_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."all_notsust"::text END) AS "NotSus_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."all_notsust"::text END) AS "NotSus_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."other_edu"::text END) AS "OtherEd_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."other_edu"::text END) AS "OtherEd_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."overall"::text END) AS "AllDest_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."overall"::text END) AS "AllDest_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."ssf"::text END) AS "SchSixthForm_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."ssf"::text END) AS "SchSixthForm_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."sfc"::text END) AS "ColSixthForm_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."sfc"::text END) AS "ColSixthForm_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."all_unknown"::text END) AS "Unknown_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."all_unknown"::text END) AS "Unknown_Dis_Est_Current_Pct_Coded"
    FROM t_ees_ks4_inst_202223_cbe11c2768 t
    GROUP BY t."school_urn"
)
,
src_2 AS (
    SELECT
        e."URN" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Number of pupils' THEN t."cohort"::text END) AS "Cohort_Tot_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202122' AND t."data_type" = 'Number of pupils' THEN t."cohort"::text END) AS "Cohort_Dis_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."overall"::text END) AS "AllDest_Tot_Est_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."overall"::text END) AS "AllDest_Dis_Est_Previous2_Pct_Coded"
    FROM t_ks4_dm_ud_202122_ins_d3152640f4 t
    JOIN v_establishment e ON (e."LAId"::text || LPAD(e."EstablishmentNumber"::text, 4, '0')) = t."school_laestab"
    GROUP BY e."URN"
)
,
src_3 AS (
    SELECT
        e."URN" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Number of pupils' THEN t."cohort"::text END) AS "Cohort_Tot_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Number of pupils' THEN t."cohort"::text END) AS "Cohort_Dis_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."overall"::text END) AS "AllDest_Tot_Est_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."data_type" = 'Percentage' THEN t."overall"::text END) AS "AllDest_Dis_Est_Previous_Pct_Coded"
    FROM t_ks4_dm_ud_202223_ins_49ea482af3 t
    JOIN v_establishment e ON (e."LAId"::text || LPAD(e."EstablishmentNumber"::text, 4, '0')) = t."school_laestab"
    GROUP BY e."URN"
)
,
all_ids AS (
    SELECT "Id" FROM src_1
    UNION SELECT "Id" FROM src_2
    UNION SELECT "Id" FROM src_3
)

SELECT
    a."Id" AS "Id",
    e."LAId" AS "LAId",
    e."LAName" AS "LAName",
    e."RegionId" AS "RegionId",
    e."RegionName" AS "RegionName",
    e."LAId" || e."EstablishmentNumber" AS "LAEstab",
    src_1."AllDest_Dis_Est_Current_Pct_Coded" AS "AllDest_Dis_Est_Current_Pct_Coded",
    src_3."AllDest_Dis_Est_Previous_Pct_Coded" AS "AllDest_Dis_Est_Previous_Pct_Coded",
    src_2."AllDest_Dis_Est_Previous2_Pct_Coded" AS "AllDest_Dis_Est_Previous2_Pct_Coded",
    src_1."AllDest_Tot_Est_Current_Pct_Coded" AS "AllDest_Tot_Est_Current_Pct_Coded",
    src_3."AllDest_Tot_Est_Previous_Pct_Coded" AS "AllDest_Tot_Est_Previous_Pct_Coded",
    src_2."AllDest_Tot_Est_Previous2_Pct_Coded" AS "AllDest_Tot_Est_Previous2_Pct_Coded",
    src_1."Apprentice_Dis_Est_Current_Pct_Coded" AS "Apprentice_Dis_Est_Current_Pct_Coded",
    src_1."Apprentice_Tot_Est_Current_Pct_Coded" AS "Apprentice_Tot_Est_Current_Pct_Coded",
    src_1."Cohort_Dis_Est_Current_Num_Coded" AS "Cohort_Dis_Est_Current_Num_Coded",
    src_3."Cohort_Dis_Est_Previous_Num_Coded" AS "Cohort_Dis_Est_Previous_Num_Coded",
    src_2."Cohort_Dis_Est_Previous2_Num_Coded" AS "Cohort_Dis_Est_Previous2_Num_Coded",
    src_1."Cohort_Tot_Est_Current_Num_Coded" AS "Cohort_Tot_Est_Current_Num_Coded",
    src_3."Cohort_Tot_Est_Previous_Num_Coded" AS "Cohort_Tot_Est_Previous_Num_Coded",
    src_2."Cohort_Tot_Est_Previous2_Num_Coded" AS "Cohort_Tot_Est_Previous2_Num_Coded",
    src_1."ColSixthForm_Dis_Est_Current_Pct_Coded" AS "ColSixthForm_Dis_Est_Current_Pct_Coded",
    src_1."ColSixthForm_Tot_Est_Current_Pct_Coded" AS "ColSixthForm_Tot_Est_Current_Pct_Coded",
    src_1."Education_Dis_Est_Current_Pct_Coded" AS "Education_Dis_Est_Current_Pct_Coded",
    src_1."Education_Tot_Est_Current_Pct_Coded" AS "Education_Tot_Est_Current_Pct_Coded",
    src_1."Employment_Dis_Est_Current_Pct_Coded" AS "Employment_Dis_Est_Current_Pct_Coded",
    src_1."Employment_Tot_Est_Current_Pct_Coded" AS "Employment_Tot_Est_Current_Pct_Coded",
    src_1."FurtherEd_Dis_Est_Current_Pct_Coded" AS "FurtherEd_Dis_Est_Current_Pct_Coded",
    src_1."FurtherEd_Tot_Est_Current_Pct_Coded" AS "FurtherEd_Tot_Est_Current_Pct_Coded",
    src_1."NotSus_Dis_Est_Current_Pct_Coded" AS "NotSus_Dis_Est_Current_Pct_Coded",
    src_1."NotSus_Tot_Est_Current_Pct_Coded" AS "NotSus_Tot_Est_Current_Pct_Coded",
    src_1."OtherEd_Dis_Est_Current_Pct_Coded" AS "OtherEd_Dis_Est_Current_Pct_Coded",
    src_1."OtherEd_Tot_Est_Current_Pct_Coded" AS "OtherEd_Tot_Est_Current_Pct_Coded",
    src_1."SchSixthForm_Dis_Est_Current_Pct_Coded" AS "SchSixthForm_Dis_Est_Current_Pct_Coded",
    src_1."SchSixthForm_Tot_Est_Current_Pct_Coded" AS "SchSixthForm_Tot_Est_Current_Pct_Coded",
    src_1."Unknown_Dis_Est_Current_Pct_Coded" AS "Unknown_Dis_Est_Current_Pct_Coded",
    src_1."Unknown_Tot_Est_Current_Pct_Coded" AS "Unknown_Tot_Est_Current_Pct_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
LEFT JOIN src_2 ON src_2."Id" = a."Id"
LEFT JOIN src_3 ON src_3."Id" = a."Id"
LEFT JOIN v_establishment e ON e."URN" = a."Id"
;
