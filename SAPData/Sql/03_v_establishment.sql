-- AUTO-GENERATED MATERIALIZED VIEW: v_establishment

DROP MATERIALIZED VIEW IF EXISTS v_establishment CASCADE;

CREATE MATERIALIZED VIEW v_establishment AS
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

    clean_int(t."resourcedprovisiononroll") AS "ResourcedProvision",
    t."typeofresourcedprovision__name_"     AS "ResourcedProvisionName",

    clean_int(t."ukprn")                    AS "UKPRN",

    t."street"                              AS "Street",
    t."locality"                            AS "Locality",
    t."address3"                            AS "Address3",
    t."town"                                AS "Town",
    t."county__name_"                       AS "County",
    t."postcode"                            AS "Postcode",

    t."headtitle__name_"                    AS "HeadTitle",
    t."headfirstname"                       AS "HeadFirstName",
    t."headlastname"                        AS "HeadLastName",
    t."headpreferredjobtitle"               AS "HeadPreferredJobTitle",

    t."urbanrural__code_"                   AS "UrbanRuralId",
    t."urbanrural__name_"                   AS "UrbanRuralName",

    t."schoolwebsite"                       AS "Website",
    clean_int(t."easting")                  AS "Easting",
    clean_int(t."northing")                 AS "Northing"
FROM t_edubasealldata202601_7caff15902 t;

CREATE UNIQUE INDEX idx_v_establishment_urn ON v_establishment ("URN");
