-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment_destinations

DROP MATERIALIZED VIEW IF EXISTS v_establishment_destinations;

CREATE MATERIALIZED VIEW v_establishment_destinations AS
WITH
src_1 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Headline destinations' AND t."destination_description" = 'Sustained apprenticeships' THEN t."pupil_percent"::text END) AS "Apprentice_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Headline destinations' AND t."destination_description" = 'Sustained apprenticeships' THEN t."pupil_percent"::text END) AS "Apprentice_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "Cohort_Tot_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202021' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "Cohort_Tot_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "Cohort_Tot_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "Cohort_Dis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202021' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "Cohort_Dis_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "Cohort_Dis_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained education destination' THEN t."pupil_percent"::text END) AS "Education_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained education destination' THEN t."pupil_percent"::text END) AS "Education_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained employment destination' THEN t."pupil_percent"::text END) AS "Employment_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained employment destination' THEN t."pupil_percent"::text END) AS "Employment_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Further education' THEN t."pupil_percent"::text END) AS "FurtherEd_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Further education' THEN t."pupil_percent"::text END) AS "FurtherEd_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Not recorded as a sustained destination' THEN t."pupil_percent"::text END) AS "NotSus_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Not recorded as a sustained destination' THEN t."pupil_percent"::text END) AS "NotSus_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Other education destination' THEN t."pupil_percent"::text END) AS "OtherEd_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Other education destination' THEN t."pupil_percent"::text END) AS "OtherEd_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."pupil_percent"::text END) AS "AllDest_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202021' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."pupil_percent"::text END) AS "AllDest_Tot_Est_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."pupil_percent"::text END) AS "AllDest_Tot_Est_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."pupil_percent"::text END) AS "AllDest_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202021' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."pupil_percent"::text END) AS "AllDest_Dis_Est_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."pupil_percent"::text END) AS "AllDest_Dis_Est_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'School sixth form' THEN t."pupil_percent"::text END) AS "SchSixthForm_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'School sixth form' THEN t."pupil_percent"::text END) AS "SchSixthForm_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sixth form college' THEN t."pupil_percent"::text END) AS "ColSixthForm_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sixth form college' THEN t."pupil_percent"::text END) AS "ColSixthForm_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Activity not captured' THEN t."pupil_percent"::text END) AS "Unknown_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Activity not captured' THEN t."pupil_percent"::text END) AS "Unknown_Dis_Est_Current_Pct_Coded"
    FROM t_ees_ks4_202223_api_0a94f12424 t
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
    src_1."AllDest_Dis_Est_Current_Pct_Coded" AS "AllDest_Dis_Est_Current_Pct_Coded",
    src_1."AllDest_Dis_Est_Previous_Pct_Coded" AS "AllDest_Dis_Est_Previous_Pct_Coded",
    src_1."AllDest_Dis_Est_Previous2_Pct_Coded" AS "AllDest_Dis_Est_Previous2_Pct_Coded",
    src_1."AllDest_Tot_Est_Current_Pct_Coded" AS "AllDest_Tot_Est_Current_Pct_Coded",
    src_1."AllDest_Tot_Est_Previous_Pct_Coded" AS "AllDest_Tot_Est_Previous_Pct_Coded",
    src_1."AllDest_Tot_Est_Previous2_Pct_Coded" AS "AllDest_Tot_Est_Previous2_Pct_Coded",
    src_1."Apprentice_Dis_Est_Current_Pct_Coded" AS "Apprentice_Dis_Est_Current_Pct_Coded",
    src_1."Apprentice_Tot_Est_Current_Pct_Coded" AS "Apprentice_Tot_Est_Current_Pct_Coded",
    src_1."Cohort_Dis_Est_Current_Num_Coded" AS "Cohort_Dis_Est_Current_Num_Coded",
    src_1."Cohort_Dis_Est_Previous_Num_Coded" AS "Cohort_Dis_Est_Previous_Num_Coded",
    src_1."Cohort_Dis_Est_Previous2_Num_Coded" AS "Cohort_Dis_Est_Previous2_Num_Coded",
    src_1."Cohort_Tot_Est_Current_Num_Coded" AS "Cohort_Tot_Est_Current_Num_Coded",
    src_1."Cohort_Tot_Est_Previous_Num_Coded" AS "Cohort_Tot_Est_Previous_Num_Coded",
    src_1."Cohort_Tot_Est_Previous2_Num_Coded" AS "Cohort_Tot_Est_Previous2_Num_Coded",
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
LEFT JOIN v_establishment e ON e."URN" = a."Id"
;
