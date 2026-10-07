-- AUTO-GENERATED MATERIALIZED VIEW: v_england_ks2_attainment

DROP MATERIALIZED VIEW IF EXISTS v_england_ks2_attainment;

CREATE MATERIALIZED VIEW v_england_ks2_attainment AS
WITH
src_1 AS (
    SELECT
        t."geographic_level" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202223' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_Eng_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202324' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_Eng_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_3YR_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged all other' AND t."subject" = 'Maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Grammar, punctuation and spelling' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTGPS_EXP_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Grammar, punctuation and spelling' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTGPS_HIGH_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202223' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202324' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_Eng_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_3YR_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_FSM6CLA1A_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged all other' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202223' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202324' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_Eng_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_3YR_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged all other' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202223' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_Eng_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202324' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_Eng_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_3YR_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged all other' AND t."subject" = 'Reading' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading' AND t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded"
    FROM t_ks2_la_nat_attainmen_e061166ae1 t
    GROUP BY t."geographic_level"
)
,
src_2 AS (
    SELECT
        t."geographic_level" AS "Id",
        MAX(CASE WHEN t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."tfsm6cla1a"::text END) AS "TFSM6CLA1A_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."tnotfsm6cla1a"::text END) AS "TNOTFSM6CLA1A_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."telig"::text END) AS "TELIG_Eng_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."geographic_level" = 'National' AND t."establishment_type_group" = 'All state funded' THEN t."totpups"::text END) AS "TOTPUPS_Eng_Current_Num_Coded"
    FROM t_ks2_la_nat_informati_7ddd755904 t
    GROUP BY t."geographic_level"
)
,
src_3 AS (
    SELECT
        t."geographic_level" AS "Id",
        MAX(CASE WHEN t."phase_type_grouping" = 'State-funded primary' AND t."hospital_school" = 'No' AND t."type_of_establishment" = 'Total' AND t."time_period" = '202425' AND t."geographic_level" = 'National' THEN t."ehc_plan_percent"::text END) AS "PSENELE_Eng_Current_Pct_Coded",
        MAX(CASE WHEN t."phase_type_grouping" = 'State-funded primary' AND t."hospital_school" = 'No' AND t."type_of_establishment" = 'Total' AND t."time_period" = '202425' AND t."geographic_level" = 'National' THEN t."sen_support_percent"::text END) AS "PSENELK_Eng_Current_Pct_Coded"
    FROM t_sen_phase_type__9c901a9464 t
    GROUP BY t."geographic_level"
)
,
src_4 AS (
    SELECT
        t."geographic_level" AS "Id",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Above Average' AND t."subject" = 'Reading' THEN t."percent_schools"::text END) AS "ProgBand_Read_Band1_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Above Average' AND t."subject" = 'Reading' THEN t."percent_schools"::text END) AS "ProgBand_Read_Band2_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Average' AND t."subject" = 'Reading' THEN t."percent_schools"::text END) AS "ProgBand_Read_Band3_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Below Average' AND t."subject" = 'Reading' THEN t."percent_schools"::text END) AS "ProgBand_Read_Band4_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Below Average' AND t."subject" = 'Reading' THEN t."percent_schools"::text END) AS "ProgBand_Read_Band5_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Above Average' AND t."subject" = 'Reading' THEN t."banding_description" END) AS "ProgBand_Read_Band1_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Above Average' AND t."subject" = 'Reading' THEN t."banding_description" END) AS "ProgBand_Read_Band2_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Average' AND t."subject" = 'Reading' THEN t."banding_description" END) AS "ProgBand_Read_Band3_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Below Average' AND t."subject" = 'Reading' THEN t."banding_description" END) AS "ProgBand_Read_Band4_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Below Average' AND t."subject" = 'Reading' THEN t."banding_description" END) AS "ProgBand_Read_Band5_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Above Average' AND t."subject" = 'Writing' THEN t."percent_schools"::text END) AS "ProgBand_Writ_Band1_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Above Average' AND t."subject" = 'Writing' THEN t."percent_schools"::text END) AS "ProgBand_Writ_Band2_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Average' AND t."subject" = 'Writing' THEN t."percent_schools"::text END) AS "ProgBand_Writ_Band3_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Below Average' AND t."subject" = 'Writing' THEN t."percent_schools"::text END) AS "ProgBand_Writ_Band4_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Below Average' AND t."subject" = 'Writing' THEN t."percent_schools"::text END) AS "ProgBand_Writ_Band5_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Above Average' AND t."subject" = 'Writing' THEN t."banding_description" END) AS "ProgBand_Writ_Band1_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Above Average' AND t."subject" = 'Writing' THEN t."banding_description" END) AS "ProgBand_Writ_Band2_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Average' AND t."subject" = 'Writing' THEN t."banding_description" END) AS "ProgBand_Writ_Band3_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Below Average' AND t."subject" = 'Writing' THEN t."banding_description" END) AS "ProgBand_Writ_Band4_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Below Average' AND t."subject" = 'Writing' THEN t."banding_description" END) AS "ProgBand_Writ_Band5_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Above Avearge' AND t."subject" = 'Maths' THEN t."percent_schools"::text END) AS "ProgBand_Math_Band1_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Above Average' AND t."subject" = 'Maths' THEN t."percent_schools"::text END) AS "ProgBand_Math_Band2_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Average' AND t."subject" = 'Maths' THEN t."percent_schools"::text END) AS "ProgBand_Math_Band3_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Below Average' AND t."subject" = 'Maths' THEN t."percent_schools"::text END) AS "ProgBand_Math_Band4_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Below Average' AND t."subject" = 'Maths' THEN t."percent_schools"::text END) AS "ProgBand_Math_Band5_Eng_Previous2_Pct_Coded",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Above Avearge' AND t."subject" = 'Maths' THEN t."banding_description" END) AS "ProgBand_Math_Band1_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Above Average' AND t."subject" = 'Maths' THEN t."banding_description" END) AS "ProgBand_Math_Band2_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Average' AND t."subject" = 'Maths' THEN t."banding_description" END) AS "ProgBand_Math_Band3_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Below Average' AND t."subject" = 'Maths' THEN t."banding_description" END) AS "ProgBand_Math_Band4_Eng_Previous2_Desc",
        MAX(CASE WHEN t."geographic_level" = 'National' AND t."time_period" = '202223' AND t."progress_banding" = 'Well Below Average' AND t."subject" = 'Maths' THEN t."banding_description" END) AS "ProgBand_Math_Band5_Eng_Previous2_Desc"
    FROM t_banding_information__086060db85 t
    GROUP BY t."geographic_level"
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
    src_1."MAT_AVERAGE_3YR_Eng_Current_Num_Coded" AS "MAT_AVERAGE_3YR_Eng_Current_Num_Coded",
    src_1."MAT_AVERAGE_Eng_Current_Num_Coded" AS "MAT_AVERAGE_Eng_Current_Num_Coded",
    src_1."MAT_AVERAGE_Eng_Previous_Num_Coded" AS "MAT_AVERAGE_Eng_Previous_Num_Coded",
    src_1."MAT_AVERAGE_Eng_Previous2_Num_Coded" AS "MAT_AVERAGE_Eng_Previous2_Num_Coded",
    src_1."MAT_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded" AS "MAT_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded",
    src_1."MAT_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded" AS "MAT_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded",
    src_4."ProgBand_Math_Band1_Eng_Previous2_Desc" AS "ProgBand_Math_Band1_Eng_Previous2_Desc",
    src_4."ProgBand_Math_Band1_Eng_Previous2_Pct_Coded" AS "ProgBand_Math_Band1_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Math_Band2_Eng_Previous2_Desc" AS "ProgBand_Math_Band2_Eng_Previous2_Desc",
    src_4."ProgBand_Math_Band2_Eng_Previous2_Pct_Coded" AS "ProgBand_Math_Band2_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Math_Band3_Eng_Previous2_Desc" AS "ProgBand_Math_Band3_Eng_Previous2_Desc",
    src_4."ProgBand_Math_Band3_Eng_Previous2_Pct_Coded" AS "ProgBand_Math_Band3_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Math_Band4_Eng_Previous2_Desc" AS "ProgBand_Math_Band4_Eng_Previous2_Desc",
    src_4."ProgBand_Math_Band4_Eng_Previous2_Pct_Coded" AS "ProgBand_Math_Band4_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Math_Band5_Eng_Previous2_Desc" AS "ProgBand_Math_Band5_Eng_Previous2_Desc",
    src_4."ProgBand_Math_Band5_Eng_Previous2_Pct_Coded" AS "ProgBand_Math_Band5_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Read_Band1_Eng_Previous2_Desc" AS "ProgBand_Read_Band1_Eng_Previous2_Desc",
    src_4."ProgBand_Read_Band1_Eng_Previous2_Pct_Coded" AS "ProgBand_Read_Band1_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Read_Band2_Eng_Previous2_Desc" AS "ProgBand_Read_Band2_Eng_Previous2_Desc",
    src_4."ProgBand_Read_Band2_Eng_Previous2_Pct_Coded" AS "ProgBand_Read_Band2_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Read_Band3_Eng_Previous2_Desc" AS "ProgBand_Read_Band3_Eng_Previous2_Desc",
    src_4."ProgBand_Read_Band3_Eng_Previous2_Pct_Coded" AS "ProgBand_Read_Band3_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Read_Band4_Eng_Previous2_Desc" AS "ProgBand_Read_Band4_Eng_Previous2_Desc",
    src_4."ProgBand_Read_Band4_Eng_Previous2_Pct_Coded" AS "ProgBand_Read_Band4_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Read_Band5_Eng_Previous2_Desc" AS "ProgBand_Read_Band5_Eng_Previous2_Desc",
    src_4."ProgBand_Read_Band5_Eng_Previous2_Pct_Coded" AS "ProgBand_Read_Band5_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Writ_Band1_Eng_Previous2_Desc" AS "ProgBand_Writ_Band1_Eng_Previous2_Desc",
    src_4."ProgBand_Writ_Band1_Eng_Previous2_Pct_Coded" AS "ProgBand_Writ_Band1_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Writ_Band2_Eng_Previous2_Desc" AS "ProgBand_Writ_Band2_Eng_Previous2_Desc",
    src_4."ProgBand_Writ_Band2_Eng_Previous2_Pct_Coded" AS "ProgBand_Writ_Band2_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Writ_Band3_Eng_Previous2_Desc" AS "ProgBand_Writ_Band3_Eng_Previous2_Desc",
    src_4."ProgBand_Writ_Band3_Eng_Previous2_Pct_Coded" AS "ProgBand_Writ_Band3_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Writ_Band4_Eng_Previous2_Desc" AS "ProgBand_Writ_Band4_Eng_Previous2_Desc",
    src_4."ProgBand_Writ_Band4_Eng_Previous2_Pct_Coded" AS "ProgBand_Writ_Band4_Eng_Previous2_Pct_Coded",
    src_4."ProgBand_Writ_Band5_Eng_Previous2_Desc" AS "ProgBand_Writ_Band5_Eng_Previous2_Desc",
    src_4."ProgBand_Writ_Band5_Eng_Previous2_Pct_Coded" AS "ProgBand_Writ_Band5_Eng_Previous2_Pct_Coded",
    src_3."PSENELE_Eng_Current_Pct_Coded" AS "PSENELE_Eng_Current_Pct_Coded",
    src_3."PSENELK_Eng_Current_Pct_Coded" AS "PSENELK_Eng_Current_Pct_Coded",
    src_1."PTGPS_EXP_Eng_Current_Pct_Coded" AS "PTGPS_EXP_Eng_Current_Pct_Coded",
    src_1."PTGPS_HIGH_Eng_Current_Pct_Coded" AS "PTGPS_HIGH_Eng_Current_Pct_Coded",
    src_1."PTRWM_EXP_3YR_Eng_Current_Pct_Coded" AS "PTRWM_EXP_3YR_Eng_Current_Pct_Coded",
    src_1."PTRWM_EXP_Eng_Current_Pct_Coded" AS "PTRWM_EXP_Eng_Current_Pct_Coded",
    src_1."PTRWM_EXP_Eng_Previous_Pct_Coded" AS "PTRWM_EXP_Eng_Previous_Pct_Coded",
    src_1."PTRWM_EXP_Eng_Previous2_Pct_Coded" AS "PTRWM_EXP_Eng_Previous2_Pct_Coded",
    src_1."PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded" AS "PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded",
    src_1."PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded" AS "PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded",
    src_1."PTRWM_HIGH_3YR_Eng_Current_Pct_Coded" AS "PTRWM_HIGH_3YR_Eng_Current_Pct_Coded",
    src_1."PTRWM_HIGH_Eng_Current_Pct_Coded" AS "PTRWM_HIGH_Eng_Current_Pct_Coded",
    src_1."PTRWM_HIGH_Eng_Previous_Pct_Coded" AS "PTRWM_HIGH_Eng_Previous_Pct_Coded",
    src_1."PTRWM_HIGH_Eng_Previous2_Pct_Coded" AS "PTRWM_HIGH_Eng_Previous2_Pct_Coded",
    src_1."PTRWM_HIGH_FSM6CLA1A_Eng_Current_Pct_Coded" AS "PTRWM_HIGH_FSM6CLA1A_Eng_Current_Pct_Coded",
    src_1."PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded" AS "PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded",
    src_1."READ_AVERAGE_3YR_Eng_Current_Num_Coded" AS "READ_AVERAGE_3YR_Eng_Current_Num_Coded",
    src_1."READ_AVERAGE_Eng_Current_Num_Coded" AS "READ_AVERAGE_Eng_Current_Num_Coded",
    src_1."READ_AVERAGE_Eng_Previous_Num_Coded" AS "READ_AVERAGE_Eng_Previous_Num_Coded",
    src_1."READ_AVERAGE_Eng_Previous2_Num_Coded" AS "READ_AVERAGE_Eng_Previous2_Num_Coded",
    src_1."READ_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded" AS "READ_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded",
    src_1."READ_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded" AS "READ_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded",
    src_2."TELIG_Eng_Current_Num_Coded" AS "TELIG_Eng_Current_Num_Coded",
    src_2."TFSM6CLA1A_Eng_Current_Num_Coded" AS "TFSM6CLA1A_Eng_Current_Num_Coded",
    src_2."TNOTFSM6CLA1A_Eng_Current_Num_Coded" AS "TNOTFSM6CLA1A_Eng_Current_Num_Coded",
    src_2."TOTPUPS_Eng_Current_Num_Coded" AS "TOTPUPS_Eng_Current_Num_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
LEFT JOIN src_2 ON src_2."Id" = a."Id"
LEFT JOIN src_3 ON src_3."Id" = a."Id"
LEFT JOIN src_4 ON src_4."Id" = a."Id"
;
