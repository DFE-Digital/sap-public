-- AUTO-GENERATED MATERIALIZED VIEW: v_la_urls

DROP MATERIALIZED VIEW IF EXISTS v_la_urls;

CREATE MATERIALIZED VIEW v_la_urls AS
WITH
src_1 AS (
    SELECT
        t."gss" AS "Id",
        MAX(CASE WHEN TRUE THEN t."authority_name" END) AS "Name",
        MAX(CASE WHEN TRUE THEN t."cropped_url" END) AS "LAMainUrl"
    FROM t_la_urls_50db45d6e3 t
    GROUP BY t."gss"
)
,
all_ids AS (
    SELECT "Id" FROM src_1
)

SELECT
    a."Id" AS "Id",
    src_1."LAMainUrl" AS "LAMainUrl",
    src_1."Name" AS "Name"
FROM all_ids a
LEFT JOIN src_1 ON src_1."Id" = a."Id"
;
