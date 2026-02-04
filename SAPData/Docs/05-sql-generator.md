
# SQL Generator

The SQL generator builds the Postgres warehouse from raw CSV files.

## Raw Tables

PostgreSQL has a 64 -character limit on identifiers and table names cannot start with a number

Prefix: t_

Format:
t_{sanitised_filename}_{10charhash}

Example:
t_edubasealldata20260128_a94f1c9a3e

Mapping file:
SAPData/Sql/tablemapping.csv

- This file contains the mapping from original file names to shortened table names - for reference.

## Views

Prefix: v_

Two types:

- DataMap-driven views
- Views created as mirrors of raw data files

## Column Handling

- Case-sensitive quoted identifiers
- Cleaned CSV header names
- IgnoreMapping = Y rows skipped, and that file is used to generate a view that mirrors the original file structure.

## Schema Strategy

Schema is not hardcoded.
Uses search_path.

## Encoding

UTF-8 without BOM

## Indexes

Explicit per-view definitions.
Uses to_regclass guards.
