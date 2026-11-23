\set ids '146529,147551'

DROP TABLE IF EXISTS export_ids;

CREATE TEMP TABLE export_ids (
    id text PRIMARY KEY
);

INSERT INTO export_ids (id)
VALUES

('100019'),
('100049'),
('100051'),
('100052'),
('100053'),
('100055'),
('100056'),
('100182'),
('100241'),
('100353'),
('100448'),
('100500'),
('100643'),
('100674'),
('100684'),
('103929'),
('103987'),
('107087'),
('109680'),
('110107'),
('111450'),
('117669'),
('124856'),
('127715'),
('130423'),
('130524'),
('130579'),
('130585'),
('135600'),
('136177'),
('136388'),
('136489'),
('136791'),
('137205'),
('137252'),
('137333'),
('137844'),
('138103'),
('138167'),
('138765'),
('139418'),
('139837'),
('140135'),
('140424'),
('140821'),
('141106'),
('141132'),
('141345'),
('141868'),
('142134'),
('142887'),
('143166'),
('143629'),
('143702'),
('144756'),
('145121'),
('146375'),
('148932'),
('149027')

;


 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment as e WHERE e."URN" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/Establishment.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_absence as e WHERE e."Id" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/EstablishmentAbsence.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_destinations as e WHERE e."Id" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS4EstablishmentDestinations.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_ks2_attainment as e WHERE e."Id" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS2EstablishmentPerformance.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_ks5_destinations as e WHERE e."Id" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS5EstablishmentDestinations.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_ks5_performance as e WHERE e."Id" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS5EstablishmentPerformance.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_ks5_subject_entries as e WHERE e."school_urn" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS5EstablishmentSubjectEntryRow.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_links as e WHERE e."urn" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/EstablishmentLinks.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_performance as e WHERE e."Id" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/EstablishmentPerformance.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_subject_entries as e WHERE e."school_urn" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS4EstablishmentSubjectEntryRow.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_establishment_top3_technical_subject_entries as e WHERE e."URN" IN (SELECT id FROM export_ids)) AS t) TO '../../SAPPub.Infrastructure/Data/Files/TechnicalSubjectEntry.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_la_absence) AS t) TO '../../SAPPub.Infrastructure/Data/Files/LAAbsence.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_la_destinations) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS4LADestinations.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_la_ks2_attainment) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS2LAPerformance.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_la_ks5_destinations) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS5LADestinations.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_la_ks5_performance) AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS5LAPerformance.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_la_performance) AS t) TO '../../SAPPub.Infrastructure/Data/Files/LAPerformance.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_la_urls) AS t) TO '../../SAPPub.Infrastructure/Data/Files/LaUrls.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_england_absence as e WHERE e."Id" ='National') AS t) TO '../../SAPPub.Infrastructure/Data/Files/EnglandAbsence.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_england_destinations as e WHERE e."Id" ='National') AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS4EnglandDestinations.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_england_ks2_attainment as e WHERE e."Id" ='National') AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS2EnglandPerformance.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_england_ks5_destinations as e WHERE e."Id" ='National') AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS5EnglandDestinations.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_england_ks5_performance as e WHERE e."Id" ='National') AS t) TO '../../SAPPub.Infrastructure/Data/Files/KS5EnglandPerformance.json'
 \copy (SELECT COALESCE(jsonb_agg(to_jsonb(t)), '[]') FROM (SELECT * FROM public.v_england_performance as e WHERE e."Id" ='National') AS t) TO '../../SAPPub.Infrastructure/Data/Files/EnglandPerformance.json'


