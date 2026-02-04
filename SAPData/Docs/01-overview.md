
# Pipeline Overview

This pipeline ingests UK school datasets, stores canonical raw files in Azure Blob Storage, generates SQL warehouse objects, and loads them into PostgreSQL running inside AKS.

## Goals

- Always ingest the latest official datasets
- Keep raw storage clean and deterministic
- Avoid hardcoded database schemas
- Make data reproducible and auditable
- Support manual historical uploads

## What Happens Each Run

1. Discover latest datasets (GIAS and EES)
2. Upload new versions to Blob Storage
3. Delete previous managed versions (after successful upload)
4. Download only current datasets (latest managed and manual_*)
5. Generate SQL (raw tables, views, indexes)
6. Execute SQL inside AKS via konduit

## Outputs

- Raw tables (t_*)
- Materialized views (v_*)
- Indexes
- Mapping metadata (SAPData/Sql/tablemapping.csv)
