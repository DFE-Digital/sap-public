-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment_performance

DROP MATERIALIZED VIEW IF EXISTS v_establishment_performance;

CREATE MATERIALIZED VIEW v_establishment_performance AS
WITH
src_1 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."progress8_banding"::text END) AS "Prog8_Banding_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."attainment8_diffn"::text END) AS "Attainment8_Diff_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."sen_with_ehcp_pupil_percent"::text END) AS "PupEHCP_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."sen_no_ehcp_pupil_percent"::text END) AS "PupSEN_Tot_Est_Current_Pct_Coded"
    FROM t_202425_information_a_34f6f3332c t
    GROUP BY t."school_urn"
)
,
src_2 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."progress8_lower_95_ci"::text END) AS "Prog8_CI_Lower_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."progress8_lower_95_ci"::text END) AS "Prog8_CI_Lower_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."progress8_lower_95_ci"::text END) AS "Prog8_CI_Lower_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."progress8_upper_95_ci"::text END) AS "Prog8_CI_Upper_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."progress8_upper_95_ci"::text END) AS "Prog8_CI_Upper_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."progress8_upper_95_ci"::text END) AS "Prog8_CI_Upper_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."progress8_average"::text END) AS "Prog8_Tot_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."progress8_average"::text END) AS "Prog8_Tot_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."progress8_average"::text END) AS "Prog8_Tot_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8_average"::text END) AS "Attainment8_Tot_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Boys' AND t."sex" = 'Boys' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8_average"::text END) AS "Attainment8_Boy_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'First language' AND t."breakdown" = 'Known or believed to be other than English' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Known or believed to be other than English' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8_average"::text END) AS "Attainment8_EAL_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8_average"::text END) AS "Attainment8_Dis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."attainment8_average"::text END) AS "Attainment8_Dis_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."attainment8_average"::text END) AS "Attainment8_Dis_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Girls' AND t."sex" = 'Girls' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8_average"::text END) AS "Attainment8_Grl_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Mobility' AND t."breakdown" = 'Non mobile' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Non mobile' AND t."time_period" = '202425' THEN t."attainment8_average"::text END) AS "Attainment8_NMo_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."attainment8_average"::text END) AS "Attainment8_Tot_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."attainment8_average"::text END) AS "Attainment8_Tot_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8ebacc_average"::text END) AS "Attainment8_Ebc_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8eng_average"::text END) AS "Attainment8_Eng_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8eng_average"::text END) AS "Attainment8_EngDis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8mat_average"::text END) AS "Attainment8_Mat_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8mat_average"::text END) AS "Attainment8_MatDis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8open_average"::text END) AS "Attainment8_AnQ_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8open_average"::text END) AS "Attainment8_AnQDis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8open_gcse_average"::text END) AS "Attainment8_Acad_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8open_gcse_average"::text END) AS "Attainment8_AcadDis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8open_nongcse_average"::text END) AS "Attainment8_Voc_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."attainment8open_nongcse_average"::text END) AS "Attainment8_VocDis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Boys' AND t."sex" = 'Boys' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."pupil_count"::text END) AS "Pup_Boy_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Girls' AND t."sex" = 'Girls' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."pupil_count"::text END) AS "Pup_Grl_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."progress8_pupil_count"::text END) AS "Prog8_TotPup_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."progress8_pupil_count"::text END) AS "Prog8_TotPup_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."progress8_pupil_count"::text END) AS "Prog8_TotPup_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Boys' AND t."sex" = 'Boys' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_94_percent"::text END) AS "EngMaths49_Boy_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Boys' AND t."sex" = 'Boys' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_95_percent"::text END) AS "EngMaths59_Boy_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Girls' AND t."sex" = 'Girls' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_94_percent"::text END) AS "EngMaths49_Grl_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Girls' AND t."sex" = 'Girls' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_95_percent"::text END) AS "EngMaths59_Grl_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."gcse_91_percent"::text END) AS "AnyQual_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_94_percent"::text END) AS "EngMaths49_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_95_percent"::text END) AS "EngMaths59_Dis_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_94_percent"::text END) AS "EngMaths49_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."engmath_94_percent"::text END) AS "EngMaths49_Tot_Est_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."engmath_94_percent"::text END) AS "EngMaths49_Tot_Est_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_95_percent"::text END) AS "EngMaths59_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."engmath_95_percent"::text END) AS "EngMaths59_Tot_Est_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."engmath_95_percent"::text END) AS "EngMaths59_Tot_Est_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'First language' AND t."breakdown" = 'Known or believed to be other than English' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Known or believed to be other than English' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_94_percent"::text END) AS "EngMaths49_EAL_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'First language' AND t."breakdown" = 'Known or believed to be other than English' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Known or believed to be other than English' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."engmath_95_percent"::text END) AS "EngMaths59_EAL_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Mobility' AND t."breakdown" = 'Non mobile' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Non mobile' AND t."time_period" = '202425' THEN t."engmath_94_percent"::text END) AS "EngMaths49_NMo_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Mobility' AND t."breakdown" = 'Non mobile' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Non mobile' AND t."time_period" = '202425' THEN t."engmath_95_percent"::text END) AS "EngMaths59_NMo_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."lan_multiple_entering_percent"::text END) AS "More1FL_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."sci_triple_entering_percent"::text END) AS "TripSci_Tot_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."qual_entries_average"::text END) AS "ExamEntriesKS4_Dis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."qual_entries_average"::text END) AS "ExamEntriesKS4_Tot_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."gcse_entries_average"::text END) AS "ExamEntriesGSCE_Tot_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."gcse_entries_average"::text END) AS "ExamEntriesGSCE_Dis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'First language' AND t."breakdown" = 'Known or believed to be other than English' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Known or believed to be other than English' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."pupil_count"::text END) AS "Pup_EAL_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantage status' AND t."breakdown" = 'Disadvantaged' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Disadvantaged' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."pupil_count"::text END) AS "Pup_Dis_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Mobility' AND t."breakdown" = 'Non mobile' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Non mobile' AND t."time_period" = '202425' THEN t."pupil_count"::text END) AS "Pup_NMo_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202425' THEN t."pupil_count"::text END) AS "Pup_Tot_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202324' THEN t."pupil_count"::text END) AS "Pup_Tot_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Total' AND t."breakdown" = 'Total' AND t."sex" = 'Total' AND t."disadvantage_status" = 'Total' AND t."first_language" = 'Total' AND t."prior_attainment" = 'Total' AND t."mobility" = 'Total' AND t."time_period" = '202223' THEN t."pupil_count"::text END) AS "Pup_Tot_Est_Previous2_Num_Coded"
    FROM t_202425_performance_t_87584e9ac7 t
    GROUP BY t."school_urn"
)
,
src_3 AS (
    SELECT
        t."urn" AS "Id",
        MAX(CASE WHEN TRUE THEN t."p8_banding" END) AS "Prog8_Banding_Est_Previous"
    FROM t_cscp_p8_ks4_2024_3a584c785a t
    GROUP BY t."urn"
)
,
src_4 AS (
    SELECT
        t."urn" AS "Id",
        MAX(CASE WHEN TRUE THEN t."p8_banding" END) AS "Prog8_Banding_Est_Previous2"
    FROM t_cscp_p8_ks4_2023_8d9da7436c t
    GROUP BY t."urn"
)
,
all_ids AS (
    SELECT "Id" FROM src_1
    UNION SELECT "Id" FROM src_2
    UNION SELECT "Id" FROM src_3
    UNION SELECT "Id" FROM src_4
)

SELECT
    a."Id" AS "Id",
    e."LAId" AS "LAId",
    e."LAName" AS "LAName",
    e."RegionId" AS "RegionId",
    e."RegionName" AS "RegionName",
    e."LAId" || e."EstablishmentNumber" AS "LAEstab",
    src_2."AnyQual_Tot_Est_Current_Pct_Coded" AS "AnyQual_Tot_Est_Current_Pct_Coded",
    src_2."Attainment8_Acad_Est_Current_Num_Coded" AS "Attainment8_Acad_Est_Current_Num_Coded",
    src_2."Attainment8_AcadDis_Est_Current_Num_Coded" AS "Attainment8_AcadDis_Est_Current_Num_Coded",
    src_2."Attainment8_AnQ_Est_Current_Num_Coded" AS "Attainment8_AnQ_Est_Current_Num_Coded",
    src_2."Attainment8_AnQDis_Est_Current_Num_Coded" AS "Attainment8_AnQDis_Est_Current_Num_Coded",
    src_2."Attainment8_Boy_Est_Current_Num_Coded" AS "Attainment8_Boy_Est_Current_Num_Coded",
    src_1."Attainment8_Diff_Est_Current_Num_Coded" AS "Attainment8_Diff_Est_Current_Num_Coded",
    src_2."Attainment8_Dis_Est_Current_Num_Coded" AS "Attainment8_Dis_Est_Current_Num_Coded",
    src_2."Attainment8_Dis_Est_Previous_Num_Coded" AS "Attainment8_Dis_Est_Previous_Num_Coded",
    src_2."Attainment8_Dis_Est_Previous2_Num_Coded" AS "Attainment8_Dis_Est_Previous2_Num_Coded",
    src_2."Attainment8_EAL_Est_Current_Num_Coded" AS "Attainment8_EAL_Est_Current_Num_Coded",
    src_2."Attainment8_Ebc_Est_Current_Num_Coded" AS "Attainment8_Ebc_Est_Current_Num_Coded",
    src_2."Attainment8_Eng_Est_Current_Num_Coded" AS "Attainment8_Eng_Est_Current_Num_Coded",
    src_2."Attainment8_EngDis_Est_Current_Num_Coded" AS "Attainment8_EngDis_Est_Current_Num_Coded",
    src_2."Attainment8_Grl_Est_Current_Num_Coded" AS "Attainment8_Grl_Est_Current_Num_Coded",
    src_2."Attainment8_Mat_Est_Current_Num_Coded" AS "Attainment8_Mat_Est_Current_Num_Coded",
    src_2."Attainment8_MatDis_Est_Current_Num_Coded" AS "Attainment8_MatDis_Est_Current_Num_Coded",
    src_2."Attainment8_NMo_Est_Current_Num_Coded" AS "Attainment8_NMo_Est_Current_Num_Coded",
    src_2."Attainment8_Tot_Est_Current_Num_Coded" AS "Attainment8_Tot_Est_Current_Num_Coded",
    src_2."Attainment8_Tot_Est_Previous_Num_Coded" AS "Attainment8_Tot_Est_Previous_Num_Coded",
    src_2."Attainment8_Tot_Est_Previous2_Num_Coded" AS "Attainment8_Tot_Est_Previous2_Num_Coded",
    src_2."Attainment8_Voc_Est_Current_Num_Coded" AS "Attainment8_Voc_Est_Current_Num_Coded",
    src_2."Attainment8_VocDis_Est_Current_Num_Coded" AS "Attainment8_VocDis_Est_Current_Num_Coded",
    src_2."EngMaths49_Boy_Est_Current_Pct_Coded" AS "EngMaths49_Boy_Est_Current_Pct_Coded",
    src_2."EngMaths49_Dis_Est_Current_Pct_Coded" AS "EngMaths49_Dis_Est_Current_Pct_Coded",
    src_2."EngMaths49_EAL_Est_Current_Pct_Coded" AS "EngMaths49_EAL_Est_Current_Pct_Coded",
    src_2."EngMaths49_Grl_Est_Current_Pct_Coded" AS "EngMaths49_Grl_Est_Current_Pct_Coded",
    src_2."EngMaths49_NMo_Est_Current_Pct_Coded" AS "EngMaths49_NMo_Est_Current_Pct_Coded",
    src_2."EngMaths49_Tot_Est_Current_Pct_Coded" AS "EngMaths49_Tot_Est_Current_Pct_Coded",
    src_2."EngMaths49_Tot_Est_Previous_Pct_Coded" AS "EngMaths49_Tot_Est_Previous_Pct_Coded",
    src_2."EngMaths49_Tot_Est_Previous2_Pct_Coded" AS "EngMaths49_Tot_Est_Previous2_Pct_Coded",
    src_2."EngMaths59_Boy_Est_Current_Pct_Coded" AS "EngMaths59_Boy_Est_Current_Pct_Coded",
    src_2."EngMaths59_Dis_Est_Current_Pct_Coded" AS "EngMaths59_Dis_Est_Current_Pct_Coded",
    src_2."EngMaths59_EAL_Est_Current_Pct_Coded" AS "EngMaths59_EAL_Est_Current_Pct_Coded",
    src_2."EngMaths59_Grl_Est_Current_Pct_Coded" AS "EngMaths59_Grl_Est_Current_Pct_Coded",
    src_2."EngMaths59_NMo_Est_Current_Pct_Coded" AS "EngMaths59_NMo_Est_Current_Pct_Coded",
    src_2."EngMaths59_Tot_Est_Current_Pct_Coded" AS "EngMaths59_Tot_Est_Current_Pct_Coded",
    src_2."EngMaths59_Tot_Est_Previous_Pct_Coded" AS "EngMaths59_Tot_Est_Previous_Pct_Coded",
    src_2."EngMaths59_Tot_Est_Previous2_Pct_Coded" AS "EngMaths59_Tot_Est_Previous2_Pct_Coded",
    src_2."ExamEntriesGSCE_Dis_Est_Current_Num_Coded" AS "ExamEntriesGSCE_Dis_Est_Current_Num_Coded",
    src_2."ExamEntriesGSCE_Tot_Est_Current_Num_Coded" AS "ExamEntriesGSCE_Tot_Est_Current_Num_Coded",
    src_2."ExamEntriesKS4_Dis_Est_Current_Num_Coded" AS "ExamEntriesKS4_Dis_Est_Current_Num_Coded",
    src_2."ExamEntriesKS4_Tot_Est_Current_Num_Coded" AS "ExamEntriesKS4_Tot_Est_Current_Num_Coded",
    src_2."More1FL_Tot_Est_Current_Pct_Coded" AS "More1FL_Tot_Est_Current_Pct_Coded",
    src_1."Prog8_Banding_Est_Current_Num_Coded" AS "Prog8_Banding_Est_Current_Num_Coded",
    src_3."Prog8_Banding_Est_Previous" AS "Prog8_Banding_Est_Previous",
    src_4."Prog8_Banding_Est_Previous2" AS "Prog8_Banding_Est_Previous2",
    src_2."Prog8_CI_Lower_Est_Current_Num_Coded" AS "Prog8_CI_Lower_Est_Current_Num_Coded",
    src_2."Prog8_CI_Lower_Est_Previous_Num_Coded" AS "Prog8_CI_Lower_Est_Previous_Num_Coded",
    src_2."Prog8_CI_Lower_Est_Previous2_Num_Coded" AS "Prog8_CI_Lower_Est_Previous2_Num_Coded",
    src_2."Prog8_CI_Upper_Est_Current_Num_Coded" AS "Prog8_CI_Upper_Est_Current_Num_Coded",
    src_2."Prog8_CI_Upper_Est_Previous_Num_Coded" AS "Prog8_CI_Upper_Est_Previous_Num_Coded",
    src_2."Prog8_CI_Upper_Est_Previous2_Num_Coded" AS "Prog8_CI_Upper_Est_Previous2_Num_Coded",
    src_2."Prog8_Tot_Est_Current_Num_Coded" AS "Prog8_Tot_Est_Current_Num_Coded",
    src_2."Prog8_Tot_Est_Previous_Num_Coded" AS "Prog8_Tot_Est_Previous_Num_Coded",
    src_2."Prog8_Tot_Est_Previous2_Num_Coded" AS "Prog8_Tot_Est_Previous2_Num_Coded",
    src_2."Prog8_TotPup_Est_Current_Num_Coded" AS "Prog8_TotPup_Est_Current_Num_Coded",
    src_2."Prog8_TotPup_Est_Previous_Num_Coded" AS "Prog8_TotPup_Est_Previous_Num_Coded",
    src_2."Prog8_TotPup_Est_Previous2_Num_Coded" AS "Prog8_TotPup_Est_Previous2_Num_Coded",
    src_2."Pup_Boy_Est_Current_Num_Coded" AS "Pup_Boy_Est_Current_Num_Coded",
    src_2."Pup_Dis_Est_Current_Num_Coded" AS "Pup_Dis_Est_Current_Num_Coded",
    src_2."Pup_EAL_Est_Current_Num_Coded" AS "Pup_EAL_Est_Current_Num_Coded",
    src_2."Pup_Grl_Est_Current_Num_Coded" AS "Pup_Grl_Est_Current_Num_Coded",
    src_2."Pup_NMo_Est_Current_Num_Coded" AS "Pup_NMo_Est_Current_Num_Coded",
    src_2."Pup_Tot_Est_Current_Num_Coded" AS "Pup_Tot_Est_Current_Num_Coded",
    src_2."Pup_Tot_Est_Previous_Num_Coded" AS "Pup_Tot_Est_Previous_Num_Coded",
    src_2."Pup_Tot_Est_Previous2_Num_Coded" AS "Pup_Tot_Est_Previous2_Num_Coded",
    src_1."PupEHCP_Tot_Est_Current_Pct_Coded" AS "PupEHCP_Tot_Est_Current_Pct_Coded",
    src_1."PupSEN_Tot_Est_Current_Pct_Coded" AS "PupSEN_Tot_Est_Current_Pct_Coded",
    src_2."TripSci_Tot_Est_Current_Pct_Coded" AS "TripSci_Tot_Est_Current_Pct_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
LEFT JOIN src_2 ON src_2."Id" = a."Id"
LEFT JOIN src_3 ON src_3."Id" = a."Id"
LEFT JOIN src_4 ON src_4."Id" = a."Id"
LEFT JOIN v_establishment e ON e."URN" = a."Id"
;
