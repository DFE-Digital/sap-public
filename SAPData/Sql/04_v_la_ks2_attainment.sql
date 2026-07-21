-- AUTO-GENERATED MATERIALIZED VIEW: v_la_ks2_attainment

DROP MATERIALIZED VIEW IF EXISTS v_la_ks2_attainment;

CREATE MATERIALIZED VIEW v_la_ks2_attainment AS
WITH
src_1 AS (
    SELECT
        t."old_la_code" AS "Id",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_23_LA_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202324' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_24_LA_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_3YR_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged all other' AND t."subject" = 'Maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "MAT_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Maths' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."progress_measure_score"::text END) AS "MATPROG_23_LA_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Grammar, punctuation and spelling' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTGPS_EXP_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Grammar, punctuation and spelling' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTGPS_HIGH_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_23_LA_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202324' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_24_LA_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_3YR_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_FSM6CLA1A_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged all other' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."expected_standard_pupil_percent"::text END) AS "PTRWM_EXP_NOTFSM6CLA1A_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_23_LA_Previous2_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202324' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_24_LA_Previous_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_3YR_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_FSM6CLA1A_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged all other' AND t."subject" = 'Reading, writing and maths' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."higher_standard_pupil_percent"::text END) AS "PTRWM_HIGH_NOTFSM6CLA1A_LA_Current_Pct_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_23_LA_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202324' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_24_LA_Previous_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Average (2023 to 2025)' AND t."breakdown" = '3 year average' AND t."subject" = 'Reading' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_3YR_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged' AND t."subject" = 'Reading' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'Disadvantaged status' AND t."breakdown" = 'Disadvantaged all other' AND t."subject" = 'Reading' AND t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."average_scaled_score"::text END) AS "READ_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Reading' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."progress_measure_score"::text END) AS "READPROG_23_LA_Previous2_Num_Coded",
        MAX(CASE WHEN t."breakdown_topic" = 'All pupils' AND t."breakdown" = 'Total' AND t."subject" = 'Writing' AND t."time_period" = '202223' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."progress_measure_score"::text END) AS "WRITPROG_23_LA_Previous2_Num_Coded"
    FROM t_ks2_la_nat_attainmen_e061166ae1 t
    GROUP BY t."old_la_code"
)
,
src_2 AS (
    SELECT
        t."old_la_code" AS "Id",
        MAX(CASE WHEN t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."tfsm6cla1a"::text END) AS "TFSM6CLA1A_LA_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' AND t."geographic_level" = 'Local authority' AND t."establishment_type_group" = 'All state funded' THEN t."tnotfsm6cla1a"::text END) AS "TNOTFSM6CLA1A_LA_Current_Num_Coded"
    FROM t_ks2_la_nat_informati_7ddd755904 t
    GROUP BY t."old_la_code"
)
,
all_ids AS (
    SELECT "Id" FROM src_1
    UNION SELECT "Id" FROM src_2
)

SELECT
    a."Id" AS "Id",
    src_1."MAT_AVERAGE_23_LA_Previous2_Num_Coded" AS "MAT_AVERAGE_23_LA_Previous2_Num_Coded",
    src_1."MAT_AVERAGE_24_LA_Previous_Num_Coded" AS "MAT_AVERAGE_24_LA_Previous_Num_Coded",
    src_1."MAT_AVERAGE_3YR_LA_Current_Num_Coded" AS "MAT_AVERAGE_3YR_LA_Current_Num_Coded",
    src_1."MAT_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded" AS "MAT_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded",
    src_1."MAT_AVERAGE_LA_Current_Num_Coded" AS "MAT_AVERAGE_LA_Current_Num_Coded",
    src_1."MAT_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded" AS "MAT_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded",
    src_1."MATPROG_23_LA_Previous2_Num_Coded" AS "MATPROG_23_LA_Previous2_Num_Coded",
    src_1."PTGPS_EXP_LA_Current_Pct_Coded" AS "PTGPS_EXP_LA_Current_Pct_Coded",
    src_1."PTGPS_HIGH_LA_Current_Pct_Coded" AS "PTGPS_HIGH_LA_Current_Pct_Coded",
    src_1."PTRWM_EXP_23_LA_Previous2_Pct_Coded" AS "PTRWM_EXP_23_LA_Previous2_Pct_Coded",
    src_1."PTRWM_EXP_24_LA_Previous_Pct_Coded" AS "PTRWM_EXP_24_LA_Previous_Pct_Coded",
    src_1."PTRWM_EXP_3YR_LA_Current_Pct_Coded" AS "PTRWM_EXP_3YR_LA_Current_Pct_Coded",
    src_1."PTRWM_EXP_FSM6CLA1A_LA_Current_Pct_Coded" AS "PTRWM_EXP_FSM6CLA1A_LA_Current_Pct_Coded",
    src_1."PTRWM_EXP_LA_Current_Pct_Coded" AS "PTRWM_EXP_LA_Current_Pct_Coded",
    src_1."PTRWM_EXP_NOTFSM6CLA1A_LA_Current_Pct_Coded" AS "PTRWM_EXP_NOTFSM6CLA1A_LA_Current_Pct_Coded",
    src_1."PTRWM_HIGH_23_LA_Previous2_Pct_Coded" AS "PTRWM_HIGH_23_LA_Previous2_Pct_Coded",
    src_1."PTRWM_HIGH_24_LA_Previous_Pct_Coded" AS "PTRWM_HIGH_24_LA_Previous_Pct_Coded",
    src_1."PTRWM_HIGH_3YR_LA_Current_Pct_Coded" AS "PTRWM_HIGH_3YR_LA_Current_Pct_Coded",
    src_1."PTRWM_HIGH_FSM6CLA1A_LA_Current_Pct_Coded" AS "PTRWM_HIGH_FSM6CLA1A_LA_Current_Pct_Coded",
    src_1."PTRWM_HIGH_LA_Current_Pct_Coded" AS "PTRWM_HIGH_LA_Current_Pct_Coded",
    src_1."PTRWM_HIGH_NOTFSM6CLA1A_LA_Current_Pct_Coded" AS "PTRWM_HIGH_NOTFSM6CLA1A_LA_Current_Pct_Coded",
    src_1."READ_AVERAGE_23_LA_Previous2_Num_Coded" AS "READ_AVERAGE_23_LA_Previous2_Num_Coded",
    src_1."READ_AVERAGE_24_LA_Previous_Num_Coded" AS "READ_AVERAGE_24_LA_Previous_Num_Coded",
    src_1."READ_AVERAGE_3YR_LA_Current_Num_Coded" AS "READ_AVERAGE_3YR_LA_Current_Num_Coded",
    src_1."READ_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded" AS "READ_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded",
    src_1."READ_AVERAGE_LA_Current_Num_Coded" AS "READ_AVERAGE_LA_Current_Num_Coded",
    src_1."READ_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded" AS "READ_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded",
    src_1."READPROG_23_LA_Previous2_Num_Coded" AS "READPROG_23_LA_Previous2_Num_Coded",
    src_2."TFSM6CLA1A_LA_Current_Num_Coded" AS "TFSM6CLA1A_LA_Current_Num_Coded",
    src_2."TNOTFSM6CLA1A_LA_Current_Num_Coded" AS "TNOTFSM6CLA1A_LA_Current_Num_Coded",
    src_1."WRITPROG_23_LA_Previous2_Num_Coded" AS "WRITPROG_23_LA_Previous2_Num_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
LEFT JOIN src_2 ON src_2."Id" = a."Id"
;
