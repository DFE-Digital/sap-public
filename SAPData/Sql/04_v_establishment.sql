-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment

DROP MATERIALIZED VIEW IF EXISTS v_establishment CASCADE;

CREATE MATERIALIZED VIEW v_establishment AS
WITH
ks4_urns AS (
    SELECT DISTINCT t."school_urn" AS "urn" FROM t_202425_information_a_34f6f3332c t
    UNION SELECT DISTINCT t."school_urn" AS "urn" FROM t_202425_performance_t_87584e9ac7 t
    UNION SELECT DISTINCT t."urn" AS "urn" FROM t_cscp_p8_ks4_2024_3a584c785a t
    UNION SELECT DISTINCT t."urn" AS "urn" FROM t_cscp_p8_ks4_2023_8d9da7436c t
),
ks5_urns AS (
    SELECT DISTINCT t."school_urn" AS "urn" FROM t_institution_performa_9b77f00557 t
)
SELECT
    t."urn"                                 AS "URN",
    t."la__code_"                           AS "LAId",
    t."la__name_"                           AS "LAName",
    clean_int(t."gor__code_")               AS "RegionId",
    t."gor__name_"                          AS "RegionName",
    t."establishmentname"                   AS "EstablishmentName",
    clean_int(t."establishmentnumber")      AS "EstablishmentNumber",

    clean_int(t."trusts__code_")            AS "TrustsId",
    t."trusts__name_"                       AS "TrustName",

    clean_int(t."admissionspolicy__code_")  AS "AdmissionsPolicyId",
    t."admissionspolicy__name_"             AS "AdmissionPolicy",

    t."districtadministrative__code_"       AS "DistrictAdministrativeId",
    t."districtadministrative__name_"       AS "DistrictAdministrativeName",

    clean_int(t."phaseofeducation__code_")  AS "PhaseOfEducationId",
    t."phaseofeducation__name_"             AS "PhaseOfEducationName",

    clean_int(t."gender__code_")            AS "GenderId",
    t."gender__name_"                       AS "GenderName",

    clean_int(t."officialsixthform__code_") AS "OfficialSixthFormId",
    clean_int(t."religiouscharacter__code_") AS "ReligiousCharacterId",
    t."religiouscharacter__name_"           AS "ReligiousCharacterName",

    t."telephonenum"                        AS "TelephoneNum",
    clean_int(t."numberofpupils")           AS "TotalPupils",

    clean_int(t."typeofestablishment__code_") AS "TypeOfEstablishmentId",
    t."typeofestablishment__name_"          AS "TypeOfEstablishmentName",

    clean_int(t."establishmenttypegroup__code_") AS "EstablishmentTypeGroupId",
    t."establishmenttypegroup__name_"          AS "EstablishmentTypeGroupName",

    clean_int(t."resourcedprovisiononroll") AS "ResourcedProvision",
    t."typeofresourcedprovision__name_"     AS "ResourcedProvisionName",

    clean_int(t."ukprn")                    AS "UKPRN",

    t."street"                              AS "AddressStreet",
    t."locality"                            AS "AddressLocality",
    t."address3"                            AS "AddressAddress3",
    t."town"                                AS "AddressTown",
    t."county__name_"                       AS "AddressCounty",
    t."postcode"                            AS "AddressPostcode",

    t."headtitle__name_"                    AS "HeadteacherTitle",
    t."headfirstname"                       AS "HeadteacherFirstName",
    t."headlastname"                        AS "HeadteacherLastName",
    t."headpreferredjobtitle"               AS "HeadteacherPreferredJobTitle",

    t."urbanrural__code_"                   AS "UrbanRuralId",
    t."urbanrural__name_"                   AS "UrbanRuralName",

    t."schoolwebsite"                       AS "Website",
    clean_int(t."easting")                  AS "Easting",
    clean_int(t."northing")                 AS "Northing",
    clean_int(t."statutorylowage")          AS "AgeRangeLow",
    clean_int(t."statutoryhighage")         AS "AgeRangeHigh",
    clean_int(t."schoolcapacity")           AS "TotalCapacity",
    clean_int(t."establishmentstatus__code_") AS "StatusCode",
    t."gsslacode__name_"                    AS "GSSLACode",
    t."closedate"                           AS "ClosedDate",
    t."opendate"                            AS "OpenDate",

    clean_int(t."reasonestablishmentopened__code_")  AS "OpenReasonId",
    t."reasonestablishmentopened__name_"             AS "OpenReasonName",
    to_tsvector('english', normalize_text(coalesce(t."establishmentname", ''))) AS "EstablishmentNameFTS",
    ST_Transform(
    ST_SetSRID(ST_MakePoint(clean_int(t."easting"), clean_int(t."northing")), 27700), 4326
)::geography AS "geom",
   NULLIF(concat_ws(', ', NULLIF(t."sen1__name_", 'Not Applicable'), NULLIF(t."sen2__name_", 'Not Applicable'), NULLIF(t."sen3__name_", 'Not Applicable'), NULLIF(t."sen4__name_", 'Not Applicable'), NULLIF(t."sen5__name_", 'Not Applicable'), NULLIF(t."sen6__name_", 'Not Applicable'), NULLIF(t."sen7__name_", 'Not Applicable'), NULLIF(t."sen8__name_", 'Not Applicable'), NULLIF(t."sen9__name_", 'Not Applicable'), NULLIF(t."sen10__name_", 'Not Applicable'), NULLIF(t."sen11__name_", 'Not Applicable'), NULLIF(t."sen12__name_", 'Not Applicable'), NULLIF(t."sen13__name_", 'Not Applicable')), '') AS "SenTypes",
    CASE WHEN (t."urn" IN (SELECT "urn" FROM ks4_urns) OR (clean_int(t."phaseofeducation__code_") IN (0, 3, 4, 5, 6, 7) AND clean_int(t."statutorylowage") < 16 AND clean_int(t."statutoryhighage") > 12) OR (clean_int(t."phaseofeducation__code_") IN (4, 7) AND clean_int(t."statutoryhighage") = 0)) THEN TRUE ELSE FALSE END AS "ISKS4",
    CASE WHEN (t."urn" IN (SELECT "urn" FROM ks5_urns) OR (clean_int(t."phaseofeducation__code_") IN (0, 4, 5, 6, 7) AND clean_int(t."statutoryhighage") > 16) OR (clean_int(t."phaseofeducation__code_") = 6 AND clean_int(t."statutoryhighage") = 0)) THEN TRUE ELSE FALSE END AS "ISKS5"

FROM t_edubasealldata202607_026196b1eb t
WHERE
    clean_int(t."phaseofeducation__code_") <> 1
    AND clean_int(t."typeofestablishment__code_") IN (1, 2, 3, 5, 6, 7, 8, 10, 11, 12, 18, 26, 28, 31, 33, 34, 35, 36, 39, 40, 41, 44, 45, 46, 56)
    AND (t."closedate" IS NULL OR t."closedate" = '' OR TO_DATE(t."closedate", 'DD/MM/YYYY') >= '2022-09-12')
    AND clean_int(t."establishmentstatus__code_") <> 4
    AND ((t."urn" IN (SELECT "urn" FROM ks4_urns) OR (clean_int(t."phaseofeducation__code_") IN (0, 3, 4, 5, 6, 7) AND clean_int(t."statutorylowage") < 16 AND clean_int(t."statutoryhighage") > 12) OR (clean_int(t."phaseofeducation__code_") IN (4, 7) AND clean_int(t."statutoryhighage") = 0)) OR (t."urn" IN (SELECT "urn" FROM ks5_urns) OR (clean_int(t."phaseofeducation__code_") IN (0, 4, 5, 6, 7) AND clean_int(t."statutoryhighage") > 16) OR (clean_int(t."phaseofeducation__code_") = 6 AND clean_int(t."statutoryhighage") = 0)))
;

CREATE UNIQUE INDEX idx_v_establishment_urn ON v_establishment ("URN");
CREATE INDEX idx_v_establishment_fts ON v_establishment USING GIN ("EstablishmentNameFTS");
CREATE INDEX idx_v_establishment_geom ON v_establishment USING GIST ("geom");
