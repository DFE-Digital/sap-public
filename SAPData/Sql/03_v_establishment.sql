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
    t."gsslacode__name_"                    AS "GSSLACode"
FROM t_edubasealldata202601_d4f37b2527 t;

CREATE UNIQUE INDEX idx_v_establishment_urn ON v_establishment ("URN");
