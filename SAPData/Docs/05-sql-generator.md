
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

## Establishment View Key Stage Filtering

The `v_establishment` materialized view includes columns such as `ISKS4` (and will include others like `ISKS2`, `ISKS5` in the future) to indicate whether each establishment is in scope for a given key stage.  
These columns are computed using key stage-specific SQL conditions, which are defined in the codebase (see `SqlViewFilterProvider`).  
The filters are based on CSCP logic using establishment attributes such as `phaseofeducation__code_`, `statutorylowage`, `statutoryhighage` and inclusion in performance files.

For more details, see the logic in:
- `SAPData/Filters/SqlViewFilterProvider.cs`
- `SAPData/GenerateViews.cs` (specifically, the generation of the `v_establishment` view and key stage filters)

This ensures that the view accurately reflects which establishments are relevant for each key stage.

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
