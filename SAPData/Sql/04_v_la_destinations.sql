-- AUTO-GENERATED MATERIALIZED VIEW: v_la_destinations

DROP MATERIALIZED VIEW IF EXISTS v_la_destinations;

CREATE MATERIALIZED VIEW v_la_destinations AS
WITH
src_1 AS (
    SELECT
        t."old_la_code" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."appren"::text END) AS "Apprentice_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."appren"::text END) AS "Apprentice_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."appren"::text END) AS "Apprentice_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Number of students' AND t."institution_group" = 'State-funded mainstream and special' THEN t."cohort"::text END) AS "Cohort_Tot_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202021' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Number of students' AND t."institution_group" = 'State-funded mainstream and special' THEN t."cohort"::text END) AS "Cohort_Tot_LA_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Number of students' AND t."institution_group" = 'State-funded mainstream and special' THEN t."cohort"::text END) AS "Cohort_Tot_LA_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Number of students' AND t."institution_group" = 'State-funded mainstream and special' THEN t."cohort"::text END) AS "Cohort_Dis_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202021' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Number of students' AND t."institution_group" = 'State-funded mainstream and special' THEN t."cohort"::text END) AS "Cohort_Dis_LA_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202122' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Number of students' AND t."institution_group" = 'State-funded mainstream and special' THEN t."cohort"::text END) AS "Cohort_Dis_LA_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."education"::text END) AS "Education_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."education"::text END) AS "Education_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."education"::text END) AS "Education_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."all_work"::text END) AS "Employment_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."all_work"::text END) AS "Employment_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."all_work"::text END) AS "Employment_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."fe"::text END) AS "FurtherEd_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."fe"::text END) AS "FurtherEd_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."fe"::text END) AS "FurtherEd_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."all_notsust"::text END) AS "NotSus_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."all_notsust"::text END) AS "NotSus_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."all_notsust"::text END) AS "NotSus_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."other_edu"::text END) AS "OtherEd_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."other_edu"::text END) AS "OtherEd_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."other_edu"::text END) AS "OtherEd_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202021' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Tot_LA_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Tot_LA_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202021' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Dis_LA_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202122' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Dis_LA_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202021' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Ndis_LA_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202122' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."overall"::text END) AS "AllDest_Ndis_LA_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."ssf"::text END) AS "SchSixthForm_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."ssf"::text END) AS "SchSixthForm_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."ssf"::text END) AS "SchSixthForm_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."sfc"::text END) AS "ColSixthForm_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."sfc"::text END) AS "ColSixthForm_Dis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Not disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."sfc"::text END) AS "Unknown_Ndis_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."all_unknown"::text END) AS "Unknown_Tot_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."data_type" = 'Percentage' AND t."institution_group" = 'State-funded mainstream and special' THEN t."all_unknown"::text END) AS "Unknown_Dis_LA_Current_Pct_Coded"
    FROM t_ees_ks4_la_202223_77d7d16802 t
    GROUP BY t."old_la_code"
)
,
all_ids AS (
    SELECT "Id" FROM src_1
)

SELECT
    a."Id" AS "Id",
    src_1."AllDest_Dis_LA_Current_Pct_Coded" AS "AllDest_Dis_LA_Current_Pct_Coded",
    src_1."AllDest_Dis_LA_Previous_Pct_Coded" AS "AllDest_Dis_LA_Previous_Pct_Coded",
    src_1."AllDest_Dis_LA_Previous2_Pct_Coded" AS "AllDest_Dis_LA_Previous2_Pct_Coded",
    src_1."AllDest_Ndis_LA_Current_Pct_Coded" AS "AllDest_Ndis_LA_Current_Pct_Coded",
    src_1."AllDest_Ndis_LA_Previous_Pct_Coded" AS "AllDest_Ndis_LA_Previous_Pct_Coded",
    src_1."AllDest_Ndis_LA_Previous2_Pct_Coded" AS "AllDest_Ndis_LA_Previous2_Pct_Coded",
    src_1."AllDest_Tot_LA_Current_Pct_Coded" AS "AllDest_Tot_LA_Current_Pct_Coded",
    src_1."AllDest_Tot_LA_Previous_Pct_Coded" AS "AllDest_Tot_LA_Previous_Pct_Coded",
    src_1."AllDest_Tot_LA_Previous2_Pct_Coded" AS "AllDest_Tot_LA_Previous2_Pct_Coded",
    src_1."Apprentice_Dis_LA_Current_Pct_Coded" AS "Apprentice_Dis_LA_Current_Pct_Coded",
    src_1."Apprentice_Ndis_LA_Current_Pct_Coded" AS "Apprentice_Ndis_LA_Current_Pct_Coded",
    src_1."Apprentice_Tot_LA_Current_Pct_Coded" AS "Apprentice_Tot_LA_Current_Pct_Coded",
    src_1."Cohort_Dis_LA_Current_Num_Coded" AS "Cohort_Dis_LA_Current_Num_Coded",
    src_1."Cohort_Dis_LA_Previous_Num_Coded" AS "Cohort_Dis_LA_Previous_Num_Coded",
    src_1."Cohort_Dis_LA_Previous2_Num_Coded" AS "Cohort_Dis_LA_Previous2_Num_Coded",
    src_1."Cohort_Tot_LA_Current_Num_Coded" AS "Cohort_Tot_LA_Current_Num_Coded",
    src_1."Cohort_Tot_LA_Previous_Num_Coded" AS "Cohort_Tot_LA_Previous_Num_Coded",
    src_1."Cohort_Tot_LA_Previous2_Num_Coded" AS "Cohort_Tot_LA_Previous2_Num_Coded",
    src_1."ColSixthForm_Dis_LA_Current_Pct_Coded" AS "ColSixthForm_Dis_LA_Current_Pct_Coded",
    src_1."ColSixthForm_Tot_LA_Current_Pct_Coded" AS "ColSixthForm_Tot_LA_Current_Pct_Coded",
    src_1."Education_Dis_LA_Current_Pct_Coded" AS "Education_Dis_LA_Current_Pct_Coded",
    src_1."Education_Ndis_LA_Current_Pct_Coded" AS "Education_Ndis_LA_Current_Pct_Coded",
    src_1."Education_Tot_LA_Current_Pct_Coded" AS "Education_Tot_LA_Current_Pct_Coded",
    src_1."Employment_Dis_LA_Current_Pct_Coded" AS "Employment_Dis_LA_Current_Pct_Coded",
    src_1."Employment_Ndis_LA_Current_Pct_Coded" AS "Employment_Ndis_LA_Current_Pct_Coded",
    src_1."Employment_Tot_LA_Current_Pct_Coded" AS "Employment_Tot_LA_Current_Pct_Coded",
    src_1."FurtherEd_Dis_LA_Current_Pct_Coded" AS "FurtherEd_Dis_LA_Current_Pct_Coded",
    src_1."FurtherEd_Ndis_LA_Current_Pct_Coded" AS "FurtherEd_Ndis_LA_Current_Pct_Coded",
    src_1."FurtherEd_Tot_LA_Current_Pct_Coded" AS "FurtherEd_Tot_LA_Current_Pct_Coded",
    src_1."NotSus_Dis_LA_Current_Pct_Coded" AS "NotSus_Dis_LA_Current_Pct_Coded",
    src_1."NotSus_Ndis_LA_Current_Pct_Coded" AS "NotSus_Ndis_LA_Current_Pct_Coded",
    src_1."NotSus_Tot_LA_Current_Pct_Coded" AS "NotSus_Tot_LA_Current_Pct_Coded",
    src_1."OtherEd_Dis_LA_Current_Pct_Coded" AS "OtherEd_Dis_LA_Current_Pct_Coded",
    src_1."OtherEd_Ndis_LA_Current_Pct_Coded" AS "OtherEd_Ndis_LA_Current_Pct_Coded",
    src_1."OtherEd_Tot_LA_Current_Pct_Coded" AS "OtherEd_Tot_LA_Current_Pct_Coded",
    src_1."SchSixthForm_Dis_LA_Current_Pct_Coded" AS "SchSixthForm_Dis_LA_Current_Pct_Coded",
    src_1."SchSixthForm_Ndis_LA_Current_Pct_Coded" AS "SchSixthForm_Ndis_LA_Current_Pct_Coded",
    src_1."SchSixthForm_Tot_LA_Current_Pct_Coded" AS "SchSixthForm_Tot_LA_Current_Pct_Coded",
    src_1."Unknown_Dis_LA_Current_Pct_Coded" AS "Unknown_Dis_LA_Current_Pct_Coded",
    src_1."Unknown_Ndis_LA_Current_Pct_Coded" AS "Unknown_Ndis_LA_Current_Pct_Coded",
    src_1."Unknown_Tot_LA_Current_Pct_Coded" AS "Unknown_Tot_LA_Current_Pct_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
;
