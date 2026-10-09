
# Developer Workflows

## Obtain data files from Blob storage

Download all .csv files including those prefixed with_manual
Copy to SAPData/DataMap/SourceFiles in SAPData project.

## Run SQL generator locally

dotnet build SAPPub.sln
dotnet run --project SAPData/SAPData.csproj

By default, only the raw tables listed in SAPData/raw_tables_to_rebuild.txt are rebuilt. 
For a full rebuild, or the first run against an empty database, set REBUILD_ALL_RAW_TABLES=true. See Selective Rebuild.

## Run SQL files against a local copy of PostgreSQL

Requires:

- psql Shell tool
- PostGIS extension installed in PostgreSQL

Run following commands in terminal:

\cd C:/Users/<username>/source/repos/sap-public/SAPData/Sql (or whatever your path to sql folder is in local SAPData project)
\i run_all.sql

## Querying data

Column names must be double-quoted because the generator preserves case-sensitive CSV headers. 
PostgreSQL lowercases unquoted identifiers by default.