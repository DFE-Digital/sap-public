-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment_ks5_destinations

DROP MATERIALIZED VIEW IF EXISTS v_establishment_ks5_destinations;

CREATE MATERIALIZED VIEW v_establishment_ks5_destinations AS
WITH
src_1 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained apprenticeships' THEN t."pupil_percent"::text END) AS "L2_APPRENPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained apprenticeships' THEN t."pupil_percent"::text END) AS "L2_APPRENPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "L2_COHORT_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "L2_COHORT_DIS_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained education destination' THEN t."pupil_percent"::text END) AS "L2_EDUCATIONPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained education destination' THEN t."pupil_percent"::text END) AS "L2_EDUCATIONPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained employment destination' THEN t."pupil_percent"::text END) AS "L2_EMPLOYMENTPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained employment destination' THEN t."pupil_percent"::text END) AS "L2_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage Status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Activity not captured' THEN t."pupil_percent"::text END) AS "L2_NOT_CAPTUREDPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Activity not captured' THEN t."pupil_percent"::text END) AS "L2_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Not recorded as a sustained destination' THEN t."pupil_percent"::text END) AS "L2_NOT_SUSTAINEDPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 2' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Not recorded as a sustained destination' THEN t."pupil_percent"::text END) AS "L2_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained apprenticeships' THEN t."pupil_percent"::text END) AS "L3_APPRENPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained apprenticeships' THEN t."pupil_percent"::text END) AS "L3_APPRENPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "L3_COHORT_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "L3_COHORT_DIS_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained education destination' THEN t."pupil_percent"::text END) AS "L3_EDUCATIONPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained education destination' THEN t."pupil_percent"::text END) AS "L3_EDUCATIONPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained employment destination' THEN t."pupil_percent"::text END) AS "L3_EMPLOYMENTPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained employment destination' THEN t."pupil_percent"::text END) AS "L3_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Activity not captured' THEN t."pupil_percent"::text END) AS "L3_NOT_CAPTUREDPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Activity not captured' THEN t."pupil_percent"::text END) AS "L3_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Not recorded as a sustained destination' THEN t."pupil_percent"::text END) AS "L3_NOT_SUSTAINEDPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Not recorded as a sustained destination' THEN t."pupil_percent"::text END) AS "L3_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Level 3' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained education, employment & apprenticeships' THEN t."pupil_percent"::text END) AS "L3_OVERALLPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained apprenticeships' THEN t."pupil_percent"::text END) AS "TOT_APPRENPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained apprenticeships' THEN t."pupil_percent"::text END) AS "TOT_APPRENPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "TOT_COHORT_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_group" = 'Overall destination' THEN t."cohort_count"::text END) AS "TOT_COHORT_DIS_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained education destination' THEN t."pupil_percent"::text END) AS "TOT_EDUCATIONPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained education destination' THEN t."pupil_percent"::text END) AS "TOT_EDUCATIONPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained employment destination' THEN t."pupil_percent"::text END) AS "TOT_EMPLOYMENTPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Sustained employment destination' THEN t."pupil_percent"::text END) AS "TOT_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Activity not captured' THEN t."pupil_percent"::text END) AS "TOT_NOT_CAPTUREDPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Activity not captured' THEN t."pupil_percent"::text END) AS "TOT_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Not recorded as a sustained destination' THEN t."pupil_percent"::text END) AS "TOT_NOT_SUSTAINEDPER_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."destination_description" = 'Not recorded as a sustained destination' THEN t."pupil_percent"::text END) AS "TOT_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202223' AND t."sex" = 'Total' AND t."cohort_level_group" = 'Total' AND t."ethnicity_major" = 'Total' AND t."disadvantage_status" = 'Total' AND t."destination_description" = 'Sustained education, employment & apprenticeships' THEN t."pupil_percent"::text END) AS "TOT_OVERALLPER_Est_Current_Pct_Coded"
    FROM t_ees_ks5_202223_api_de3fce6354 t
    GROUP BY t."school_urn"
)
,
src_2 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Number of pupils' THEN t."acag_cohort"::text END) AS "ACADAGEN_COHORT_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."acag_progressed"::text END) AS "ACADAGEN_PROGRESSED_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."all_appren"::text END) AS "ALL_APPREN_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Number of pupils' THEN t."all_cohort"::text END) AS "ALL_COHORT_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."all_degree"::text END) AS "ALL_HE_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."all_htech"::text END) AS "ALL_HTECH_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."all_progressed"::text END) AS "ALL_PROGRESSED_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."all_top3rd"::text END) AS "ALL_TOP3RD_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."all_degree"::text END) AS "DIS_HE_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."all_htech"::text END) AS "DIS_HTECH_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."all_top3rd"::text END) AS "DIS_TOP3RD_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Number of pupils' THEN t."otl3_cohort"::text END) AS "OTHER_COHORT_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."otl3_progressed"::text END) AS "OTHER_PROGRESSED_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Number of pupils' THEN t."tlev_cohort"::text END) AS "TLEV_COHORT_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."time_period" = '202122' AND t."data_type" = 'Percentage' THEN t."tlev_progressed"::text END) AS "TLEV_PROGRESSED_Est_Current_Num_Coded"
    FROM t_l4_tidy_2024_all_ins_73b15a1581 t
    GROUP BY t."school_urn"
)
,
all_ids AS (
    SELECT "Id" FROM src_1
    UNION SELECT "Id" FROM src_2
)

SELECT
    a."Id" AS "Id",
    e."LAId" AS "LAId",
    e."LAName" AS "LAName",
    e."RegionId" AS "RegionId",
    e."RegionName" AS "RegionName",
    e."LAId" || e."EstablishmentNumber" AS "LAEstab",
    src_2."ACADAGEN_COHORT_Est_Current_Num_Coded" AS "ACADAGEN_COHORT_Est_Current_Num_Coded",
    src_2."ACADAGEN_PROGRESSED_Est_Current_Num_Coded" AS "ACADAGEN_PROGRESSED_Est_Current_Num_Coded",
    src_2."ALL_APPREN_Est_Current_Num_Coded" AS "ALL_APPREN_Est_Current_Num_Coded",
    src_2."ALL_COHORT_Est_Current_Num_Coded" AS "ALL_COHORT_Est_Current_Num_Coded",
    src_2."ALL_HE_Est_Current_Num_Coded" AS "ALL_HE_Est_Current_Num_Coded",
    src_2."ALL_HTECH_Est_Current_Num_Coded" AS "ALL_HTECH_Est_Current_Num_Coded",
    src_2."ALL_PROGRESSED_Est_Current_Num_Coded" AS "ALL_PROGRESSED_Est_Current_Num_Coded",
    src_2."ALL_TOP3RD_Est_Current_Num_Coded" AS "ALL_TOP3RD_Est_Current_Num_Coded",
    src_2."DIS_HE_Est_Current_Num_Coded" AS "DIS_HE_Est_Current_Num_Coded",
    src_2."DIS_HTECH_Est_Current_Num_Coded" AS "DIS_HTECH_Est_Current_Num_Coded",
    src_2."DIS_TOP3RD_Est_Current_Num_Coded" AS "DIS_TOP3RD_Est_Current_Num_Coded",
    src_1."L2_APPRENPER_DIS_Est_Current_Pct_Coded" AS "L2_APPRENPER_DIS_Est_Current_Pct_Coded",
    src_1."L2_APPRENPER_Est_Current_Pct_Coded" AS "L2_APPRENPER_Est_Current_Pct_Coded",
    src_1."L2_COHORT_DIS_Est_Current_Num_Coded" AS "L2_COHORT_DIS_Est_Current_Num_Coded",
    src_1."L2_COHORT_Est_Current_Num_Coded" AS "L2_COHORT_Est_Current_Num_Coded",
    src_1."L2_EDUCATIONPER_DIS_Est_Current_Pct_Coded" AS "L2_EDUCATIONPER_DIS_Est_Current_Pct_Coded",
    src_1."L2_EDUCATIONPER_Est_Current_Pct_Coded" AS "L2_EDUCATIONPER_Est_Current_Pct_Coded",
    src_1."L2_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded" AS "L2_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded",
    src_1."L2_EMPLOYMENTPER_Est_Current_Pct_Coded" AS "L2_EMPLOYMENTPER_Est_Current_Pct_Coded",
    src_1."L2_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded" AS "L2_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded",
    src_1."L2_NOT_CAPTUREDPER_Est_Current_Pct_Coded" AS "L2_NOT_CAPTUREDPER_Est_Current_Pct_Coded",
    src_1."L2_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded" AS "L2_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded",
    src_1."L2_NOT_SUSTAINEDPER_Est_Current_Pct_Coded" AS "L2_NOT_SUSTAINEDPER_Est_Current_Pct_Coded",
    src_1."L3_APPRENPER_DIS_Est_Current_Pct_Coded" AS "L3_APPRENPER_DIS_Est_Current_Pct_Coded",
    src_1."L3_APPRENPER_Est_Current_Pct_Coded" AS "L3_APPRENPER_Est_Current_Pct_Coded",
    src_1."L3_COHORT_DIS_Est_Current_Num_Coded" AS "L3_COHORT_DIS_Est_Current_Num_Coded",
    src_1."L3_COHORT_Est_Current_Num_Coded" AS "L3_COHORT_Est_Current_Num_Coded",
    src_1."L3_EDUCATIONPER_DIS_Est_Current_Pct_Coded" AS "L3_EDUCATIONPER_DIS_Est_Current_Pct_Coded",
    src_1."L3_EDUCATIONPER_Est_Current_Pct_Coded" AS "L3_EDUCATIONPER_Est_Current_Pct_Coded",
    src_1."L3_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded" AS "L3_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded",
    src_1."L3_EMPLOYMENTPER_Est_Current_Pct_Coded" AS "L3_EMPLOYMENTPER_Est_Current_Pct_Coded",
    src_1."L3_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded" AS "L3_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded",
    src_1."L3_NOT_CAPTUREDPER_Est_Current_Pct_Coded" AS "L3_NOT_CAPTUREDPER_Est_Current_Pct_Coded",
    src_1."L3_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded" AS "L3_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded",
    src_1."L3_NOT_SUSTAINEDPER_Est_Current_Pct_Coded" AS "L3_NOT_SUSTAINEDPER_Est_Current_Pct_Coded",
    src_1."L3_OVERALLPER_Est_Current_Pct_Coded" AS "L3_OVERALLPER_Est_Current_Pct_Coded",
    src_2."OTHER_COHORT_Est_Current_Num_Coded" AS "OTHER_COHORT_Est_Current_Num_Coded",
    src_2."OTHER_PROGRESSED_Est_Current_Num_Coded" AS "OTHER_PROGRESSED_Est_Current_Num_Coded",
    src_2."TLEV_COHORT_Est_Current_Num_Coded" AS "TLEV_COHORT_Est_Current_Num_Coded",
    src_2."TLEV_PROGRESSED_Est_Current_Num_Coded" AS "TLEV_PROGRESSED_Est_Current_Num_Coded",
    src_1."TOT_APPRENPER_DIS_Est_Current_Pct_Coded" AS "TOT_APPRENPER_DIS_Est_Current_Pct_Coded",
    src_1."TOT_APPRENPER_Est_Current_Pct_Coded" AS "TOT_APPRENPER_Est_Current_Pct_Coded",
    src_1."TOT_COHORT_DIS_Est_Current_Num_Coded" AS "TOT_COHORT_DIS_Est_Current_Num_Coded",
    src_1."TOT_COHORT_Est_Current_Num_Coded" AS "TOT_COHORT_Est_Current_Num_Coded",
    src_1."TOT_EDUCATIONPER_DIS_Est_Current_Pct_Coded" AS "TOT_EDUCATIONPER_DIS_Est_Current_Pct_Coded",
    src_1."TOT_EDUCATIONPER_Est_Current_Pct_Coded" AS "TOT_EDUCATIONPER_Est_Current_Pct_Coded",
    src_1."TOT_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded" AS "TOT_EMPLOYMENTPER_DIS_Est_Current_Pct_Coded",
    src_1."TOT_EMPLOYMENTPER_Est_Current_Pct_Coded" AS "TOT_EMPLOYMENTPER_Est_Current_Pct_Coded",
    src_1."TOT_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded" AS "TOT_NOT_CAPTUREDPER_DIS_Est_Current_Pct_Coded",
    src_1."TOT_NOT_CAPTUREDPER_Est_Current_Pct_Coded" AS "TOT_NOT_CAPTUREDPER_Est_Current_Pct_Coded",
    src_1."TOT_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded" AS "TOT_NOT_SUSTAINEDPER_DIS_Est_Current_Pct_Coded",
    src_1."TOT_NOT_SUSTAINEDPER_Est_Current_Pct_Coded" AS "TOT_NOT_SUSTAINEDPER_Est_Current_Pct_Coded",
    src_1."TOT_OVERALLPER_Est_Current_Pct_Coded" AS "TOT_OVERALLPER_Est_Current_Pct_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
LEFT JOIN src_2 ON src_2."Id" = a."Id"
LEFT JOIN v_establishment e ON e."URN" = a."Id"
;
