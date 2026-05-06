-- =========================
-- Synonym table for text search normalization
-- =========================

DROP TABLE IF EXISTS public.thesaurus_synonyms CASCADE;
CREATE TABLE public.thesaurus_synonyms (
    word text PRIMARY KEY,
    synonym text NOT NULL
);

INSERT INTO public.thesaurus_synonyms (word, synonym) VALUES
('rc', 'catholic'),
('roman catholic', 'catholic'),
('catholic', 'catholic'),
('cofe', 'cofe'),
('church of england', 'cofe'),
('saint', 'st'),
('saints', 'ss');