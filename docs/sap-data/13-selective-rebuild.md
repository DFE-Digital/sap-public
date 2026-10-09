# Selective Raw Table Rebuild

By default, the pipeline no longer drops and reloads every raw table on each run.
It rebuilds only the raw tables listed in the rebuild list, then regenerates only the materialized views that read those tables. 
Everything else is left in place.

## Why

Most source files change rarely. Reloading all of them daily is slow and risked wiping data that had not changed. The daily run
now only touches the GIAS and EES API datasets (refreshed when a new version is published). Additionally, the infrastructure 
via Konduit has a tendency to be slow, clunky and periodically times out when running the pipeline.

## The rebuild list

One file per environment, chosen by the workflow from the `environment` input:

| Environment | File |
| --- | --- |
| test | SAPData/raw_tables_to_rebuild.test.txt |
| review | SAPData/raw_tables_to_rebuild.review.txt |
| production | SAPData/raw_tables_to_rebuild.production.txt |
| local | SAPData/raw_tables_to_rebuild.txt |

Format: One logical dataset key per line. Lines starting with `#` are ignored. A key is the source filename without the .csv extension and without any manual_ prefix.

| Form | Example | Matches |
| --- | --- | --- |
| GIAS template | `edubasealldataYYYYmmDD` | any file with an 8-digit date in place of the token
| EES unversioned name | `202425_subject_school_all_exame_entriesgrades_provisional` | that name with any `_v1.01 style suffix` |
| Exact key | `ks2_wraparound_care` | that file only (use for manual uploads)

Match the filename, not the `t_` table name.

## Configuration

| Setting | Environment variable | Effect
| --- | --- | --- |
| Rebuild list path | `RAW_TABLES_TO_REBUILD_PATH` | Overrides the list file. Relative paths resolve from the SAPData folder |
| Rebuild everything | `REBUILD_ALL_RAW_TABLES` | Ignores the list. Drops every `t_` table and regenerates every view |

Both can be set as user secrets for local runs, they would be (on the SAPData project):

```
{
  "RebuildAllRawTables": false,
  "RawTablesToRebuildPath": "raw_tables_to_rebuild.txt"
}
```

## What the generator does

1. `GenerateRawTables` reads every CSV in sourcefiles and records its t_ table name in tablemapping.csv (regardless of if its being rebuild or not). Only listted files are cleaned and only they get `DROP TABLE`, `CREATE TABLE` and `COPY` statements.
2. `00_cleanup.sql` drops two sets of t_ tables: those being rebuilt and any t_ table in the schema that no current source file maps to.
The second set cleans up old tables left behind when a GIAS date or EES version changes. Stale drops are logged with `NOTICE`.
3. `GenerateViews` receives the phyical names of the tables that were actually rebuilt. A view is regnerated if any raw table it reads is in that set.
`v_establishment` is also regenerated if any of its lookup tables changed (KS performance, breakfast club, wraparound care).
4. Views that are not regenerated are written as a stub.

Drop a raw table uses `CASCADE`, which also drops any materialized view reading it. Step 3 is what guarantees those view are recreated in the same run.

## First run on an empty database

Selected mode assumes every untouched view already exists. On a fresh database, or after restoring an old backup, run once with `REBUILD_ALL_RAW_TABLES=true`.
Review apps get a seed database for this reason.


## Adding or re-uploading a manual file

1. Upload the file to blob storage with the `manual_` prefix
2. Add its logical key (no prefix, no extension) to the rebuild list for the target environment
3. Run the pipeline
4. Remove the key from the list again so it isn't reloaded all the time

## Checking a generated run

In `SAPData/Sql` after `dotnet run`:

- `00_cleanup.sql` lists `tables_to_rebuild` and `tables_to_keep` near the top
- `02_create_raw_tables.sql` should contain only the tables being rebuilt
- Each `04_v_*.sql` either starts with `CREATE MATERIALIZED VIEW` or is a stub whose header says why it was skipped

## Common failure

```
ERROR: Skipped view v_establishment does not exist in schema public, but is required for selective rebuild.
```

The view was dropped by `CASCADE` in cleanup, but the generator decided it not need regenerating. Causes, in order of likelihood:

- The databse was empty or was restored from before the view existed. Run with `REBUILD_ALL_RAW_TABLES=true`
- The rebuild list entry does not match the filename in `SourceFiles`. Compare the list against `tablemapping.csv`
- A code change in `GenerateViews` broke the match between rebuilt table names and the names it checks.