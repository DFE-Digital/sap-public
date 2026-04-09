-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment_workforce

DROP MATERIALIZED VIEW IF EXISTS v_establishment_workforce;

CREATE MATERIALIZED VIEW v_establishment_workforce AS
WITH
src_1 AS (
    SELECT
        t."school_urn" AS "Id",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."pupil_to_qual_teacher_ratio"::text END) AS "Workforce_PupTeaRatio_Est_Current_Num_Coded",
        MAX(CASE WHEN t."time_period" = '202425' THEN t."pupils_fte"::text END) AS "Workforce_TotPupils_Est_Current_Num_Coded"
    FROM t_workforce_ptrs_2010__8b26fc7d53 t
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
    src_1."Workforce_PupTeaRatio_Est_Current_Num_Coded" AS "Workforce_PupTeaRatio_Est_Current_Num_Coded",
    src_1."Workforce_TotPupils_Est_Current_Num_Coded" AS "Workforce_TotPupils_Est_Current_Num_Coded"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
LEFT JOIN v_establishment e ON e."URN" = a."Id"
;
