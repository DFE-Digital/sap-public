
# Architecture

## Components

GitHub Actions
PowerShell Versioning (Step 6)
Azure Blob Storage
Controlled Download (Step 7)
.NET 8 SQL Generator
PostgreSQL via AKS and konduit

## Storage Layers

Azure Blob Storage stores:

- Managed datasets (latest only)
- Manual datasets (manual_* prefix - used to easily identify files that are uploaded once and never updated e.g. historical data)
- versions.json manifest

Blob storage is the authoritative raw data store.

## Database

PostgreSQL contains:

- Raw tables (prefix t_)
- Materialized views (prefix v_)

Schema is not hardcoded and uses search_path.

## Security Model

Secrets are injected via GitHub Actions:

- Azure auth uses OIDC
- Sensitive dataset URL templates are stored in GitHub secrets
- Blob access via connection string stored in GitHub secrets
