-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment_ks2_attainment

DROP MATERIALIZED VIEW IF EXISTS v_establishment_ks2_attainment;

CREATE MATERIALIZED VIEW v_establishment_ks2_attainment AS
WITH
src_1 AS (
    SELECT
        t."urn" AS "Id",
        MAX(CASE WHEN TRUE THEN t."matprog_descr"::text END) AS "MATPROG_DESCR_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN TRUE THEN t."readprog_descr"::text END) AS "READPROG_DESCR_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN TRUE THEN t."writprog_descr"::text END) AS "WRITPROG_DESCR_23_Est_Previous2_Num_Coded"
    FROM t_cscp_prog_ks2_2023_b7d7dc9325 t
    GROUP BY t."urn"
)
,
src_2 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202223' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202324' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_24_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Maths' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_3YR_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Boys' AND t."subject" = 'Maths' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_B_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'First language' AND t."breakdown" = 'Known or believed to be other than English' AND t."subject" = 'Maths' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_EAL_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Maths' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Girls' AND t."subject" = 'Maths' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_G_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Mobility status' AND t."breakdown" = 'Non mobile' AND t."subject" = 'Maths' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_MOBN_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202223' THEN t."progress_measure_score"::text END) AS "MATPROG_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202223' THEN t."progress_measure_lower_conf_interval"::text END) AS "MATPROG_LOWER_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202223' THEN t."progress_measure_upper_conf_interval"::text END) AS "MATPROG_UPPER_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Grammar, punctuation and spelling' AND t."time_period" = '202425' THEN t."expected_standard_pupil_percent"::text END) AS "PTGPS_EXP_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Grammar, punctuation and spelling' AND t."time_period" = '202425' THEN t."higher_standard_pupil_percent"::text END) AS "PTGPS_HIGH_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202223' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_23_Est_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202324' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_24_Est_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_3YR_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Boys' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_B_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'First language' AND t."breakdown" = 'Known or believed to be other than English' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_EAL_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_FSM6CLA1A_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Girls' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_G_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Mobility status' AND t."breakdown" = 'Non mobile' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_MOBN_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202223' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_23_Est_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202324' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_24_Est_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_3YR_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Boys' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_B_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'First language' AND t."breakdown" = 'Known or believed to be other than English' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_EAL_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_FSM6CLA1A_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Girls' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_G_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Mobility status' AND t."breakdown" = 'Non mobile' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_MOBN_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202223' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202324' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_24_Est_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_3YR_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Sex' AND t."breakdown" = 'Boys' AND t."subject" = 'Reading' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_B_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'First language' AND t."breakdown" = 'Known or believed to be other than English' AND t."subject" = 'Reading' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_EAL_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_G_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Mobility status' AND t."breakdown" = 'Non mobile' AND t."subject" = 'Reading' AND t."time_period" = '202425' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_MOBN_Est_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202223' THEN t."progress_measure_score"::text END) AS "READPROG_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202223' THEN t."progress_measure_lower_conf_interval"::text END) AS "READPROG_LOWER_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202223' THEN t."progress_measure_upper_conf_interval"::text END) AS "READPROG_UPPER_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Writing' AND t."time_period" = '202223' THEN t."progress_measure_score"::text END) AS "WRITPROG_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Writing' AND t."time_period" = '202223' THEN t."progress_measure_lower_conf_interval"::text END) AS "WRITPROG_LOWER_23_Est_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Writing' AND t."time_period" = '202223' THEN t."progress_measure_upper_conf_interval"::text END) AS "WRITPROG_UPPER_23_Est_Previous2_Num_Coded"
    FROM t_ks2_school_attainmen_74fad1645e t
    GROUP BY t."school_urn"
)
,
src_3 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."belig"::text END) AS "BELIG_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."gelig"::text END) AS "GELIG_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."psenele"::text END) AS "PSENELE_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."psenelk"::text END) AS "PSENELK_Est_Current_Pct_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."tealgrp2"::text END) AS "TEALGRP2_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."telig"::text END) AS "TELIG_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."tfsm6cla1a"::text END) AS "TFSM6CLA1A_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."tmobn"::text END) AS "TMOBN_Est_Current_Num_Coded"
    FROM t_ks2_school_informati_bd92f37de7 t
    GROUP BY t."school_urn"
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
    src_3."BELIG_Est_Current_Num_Coded" AS "BELIG_Est_Current_Num_Coded",
    src_3."GELIG_Est_Current_Num_Coded" AS "GELIG_Est_Current_Num_Coded",
    src_2."MAT_AVERAGE_23_Est_Previous2_Num_Coded" AS "MAT_AVERAGE_23_Est_Previous2_Num_Coded",
    src_2."MAT_AVERAGE_24_Est_Previous_Num_Coded" AS "MAT_AVERAGE_24_Est_Previous_Num_Coded",
    src_2."MAT_AVERAGE_3YR_Est_Current_Num_Coded" AS "MAT_AVERAGE_3YR_Est_Current_Num_Coded",
    src_2."MAT_AVERAGE_B_Est_Current_Num_Coded" AS "MAT_AVERAGE_B_Est_Current_Num_Coded",
    src_2."MAT_AVERAGE_EAL_Est_Current_Num_Coded" AS "MAT_AVERAGE_EAL_Est_Current_Num_Coded",
    src_2."MAT_AVERAGE_Est_Current_Num_Coded" AS "MAT_AVERAGE_Est_Current_Num_Coded",
    src_2."MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded" AS "MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded",
    src_2."MAT_AVERAGE_G_Est_Current_Num_Coded" AS "MAT_AVERAGE_G_Est_Current_Num_Coded",
    src_2."MAT_AVERAGE_MOBN_Est_Current_Num_Coded" AS "MAT_AVERAGE_MOBN_Est_Current_Num_Coded",
    src_2."MATPROG_23_Est_Previous2_Num_Coded" AS "MATPROG_23_Est_Previous2_Num_Coded",
    src_1."MATPROG_DESCR_23_Est_Previous2_Num_Coded" AS "MATPROG_DESCR_23_Est_Previous2_Num_Coded",
    src_2."MATPROG_LOWER_23_Est_Previous2_Num_Coded" AS "MATPROG_LOWER_23_Est_Previous2_Num_Coded",
    src_2."MATPROG_UPPER_23_Est_Previous2_Num_Coded" AS "MATPROG_UPPER_23_Est_Previous2_Num_Coded",
    src_3."PSENELE_Est_Current_Pct_Coded" AS "PSENELE_Est_Current_Pct_Coded",
    src_3."PSENELK_Est_Current_Pct_Coded" AS "PSENELK_Est_Current_Pct_Coded",
    src_2."PTGPS_EXP_Est_Current_Pct_Coded" AS "PTGPS_EXP_Est_Current_Pct_Coded",
    src_2."PTGPS_HIGH_Est_Current_Pct_Coded" AS "PTGPS_HIGH_Est_Current_Pct_Coded",
    src_2."PTRWM_EXP_23_Est_Previous2_Pct_Coded" AS "PTRWM_EXP_23_Est_Previous2_Pct_Coded",
    src_2."PTRWM_EXP_24_Est_Previous_Pct_Coded" AS "PTRWM_EXP_24_Est_Previous_Pct_Coded",
    src_2."PTRWM_EXP_3YR_Est_Current_Pct_Coded" AS "PTRWM_EXP_3YR_Est_Current_Pct_Coded",
    src_2."PTRWM_EXP_B_Est_Current_Pct_Coded" AS "PTRWM_EXP_B_Est_Current_Pct_Coded",
    src_2."PTRWM_EXP_EAL_Est_Current_Pct_Coded" AS "PTRWM_EXP_EAL_Est_Current_Pct_Coded",
    src_2."PTRWM_EXP_Est_Current_Pct_Coded" AS "PTRWM_EXP_Est_Current_Pct_Coded",
    src_2."PTRWM_EXP_FSM6CLA1A_Est_Current_Pct_Coded" AS "PTRWM_EXP_FSM6CLA1A_Est_Current_Pct_Coded",
    src_2."PTRWM_EXP_G_Est_Current_Pct_Coded" AS "PTRWM_EXP_G_Est_Current_Pct_Coded",
    src_2."PTRWM_EXP_MOBN_Est_Current_Pct_Coded" AS "PTRWM_EXP_MOBN_Est_Current_Pct_Coded",
    src_2."PTRWM_HIGH_23_Est_Previous2_Pct_Coded" AS "PTRWM_HIGH_23_Est_Previous2_Pct_Coded",
    src_2."PTRWM_HIGH_24_Est_Previous_Pct_Coded" AS "PTRWM_HIGH_24_Est_Previous_Pct_Coded",
    src_2."PTRWM_HIGH_3YR_Est_Current_Pct_Coded" AS "PTRWM_HIGH_3YR_Est_Current_Pct_Coded",
    src_2."PTRWM_HIGH_B_Est_Current_Pct_Coded" AS "PTRWM_HIGH_B_Est_Current_Pct_Coded",
    src_2."PTRWM_HIGH_EAL_Est_Current_Pct_Coded" AS "PTRWM_HIGH_EAL_Est_Current_Pct_Coded",
    src_2."PTRWM_HIGH_Est_Current_Pct_Coded" AS "PTRWM_HIGH_Est_Current_Pct_Coded",
    src_2."PTRWM_HIGH_FSM6CLA1A_Est_Current_Pct_Coded" AS "PTRWM_HIGH_FSM6CLA1A_Est_Current_Pct_Coded",
    src_2."PTRWM_HIGH_G_Est_Current_Pct_Coded" AS "PTRWM_HIGH_G_Est_Current_Pct_Coded",
    src_2."PTRWM_HIGH_MOBN_Est_Current_Pct_Coded" AS "PTRWM_HIGH_MOBN_Est_Current_Pct_Coded",
    src_2."READ_AVERAGE_23_Est_Previous2_Num_Coded" AS "READ_AVERAGE_23_Est_Previous2_Num_Coded",
    src_2."READ_AVERAGE_24_Est_Previous_Num_Coded" AS "READ_AVERAGE_24_Est_Previous_Num_Coded",
    src_2."READ_AVERAGE_3YR_Est_Current_Num_Coded" AS "READ_AVERAGE_3YR_Est_Current_Num_Coded",
    src_2."READ_AVERAGE_B_Est_Current_Num_Coded" AS "READ_AVERAGE_B_Est_Current_Num_Coded",
    src_2."READ_AVERAGE_EAL_Est_Current_Num_Coded" AS "READ_AVERAGE_EAL_Est_Current_Num_Coded",
    src_2."READ_AVERAGE_Est_Current_Num_Coded" AS "READ_AVERAGE_Est_Current_Num_Coded",
    src_2."READ_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded" AS "READ_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded",
    src_2."READ_AVERAGE_G_Est_Current_Num_Coded" AS "READ_AVERAGE_G_Est_Current_Num_Coded",
    src_2."READ_AVERAGE_MOBN_Est_Current_Num_Coded" AS "READ_AVERAGE_MOBN_Est_Current_Num_Coded",
    src_2."READPROG_23_Est_Previous2_Num_Coded" AS "READPROG_23_Est_Previous2_Num_Coded",
    src_1."READPROG_DESCR_23_Est_Previous2_Num_Coded" AS "READPROG_DESCR_23_Est_Previous2_Num_Coded",
    src_2."READPROG_LOWER_23_Est_Previous2_Num_Coded" AS "READPROG_LOWER_23_Est_Previous2_Num_Coded",
    src_2."READPROG_UPPER_23_Est_Previous2_Num_Coded" AS "READPROG_UPPER_23_Est_Previous2_Num_Coded",
    src_3."TEALGRP2_Est_Current_Num_Coded" AS "TEALGRP2_Est_Current_Num_Coded",
    src_3."TELIG_Est_Current_Num_Coded" AS "TELIG_Est_Current_Num_Coded",
    src_3."TFSM6CLA1A_Est_Current_Num_Coded" AS "TFSM6CLA1A_Est_Current_Num_Coded",
    src_3."TMOBN_Est_Current_Num_Coded" AS "TMOBN_Est_Current_Num_Coded",
    src_2."WRITPROG_23_Est_Previous2_Num_Coded" AS "WRITPROG_23_Est_Previous2_Num_Coded",
    src_1."WRITPROG_DESCR_23_Est_Previous2_Num_Coded" AS "WRITPROG_DESCR_23_Est_Previous2_Num_Coded",
    src_2."WRITPROG_LOWER_23_Est_Previous2_Num_Coded" AS "WRITPROG_LOWER_23_Est_Previous2_Num_Coded",
    src_2."WRITPROG_UPPER_23_Est_Previous2_Num_Coded" AS "WRITPROG_UPPER_23_Est_Previous2_Num_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
LEFT JOIN src_2 ON src_2."Id" = a."Id"
LEFT JOIN src_3 ON src_3."Id" = a."Id"
LEFT JOIN v_establishment e ON e."URN" = a."Id"
;
