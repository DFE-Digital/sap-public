# KS2 EES API PR Review and ETL Comparison

## Executive summary

This PR introduces a new KS2 data retrieval path that reads directly from the EES API at runtime, instead of using the existing Postgres-backed ingestion flow for KS2 endpoints.

At a high level, it adds:

- A new EES integration layer (client, request/response models, transformations, lookup service)
- KS2 DTOs decorated with `EESInput` mappings
- A KS2 repository implementation that maps EES responses into existing domain models
- A metadata/code generation project to generate typed EES filter/indicator constants from EES metadata
- Dependency injection changes so KS2 now resolves to the EES-backed repository

`git diff --stat origin/main...HEAD` shows **38 files changed** and **3,453 insertions**.

## What changed in the application

## 1) Data access path for KS2

The DI registration now binds:

- `IKS2PerformanceRepository` -> `KS2PerformanceEESRepository`

This is a functional shift from the previous repository (`Ks2PerformanceRepository`) that read from generic storage abstractions (JSON/Postgres depending on environment).

## 2) New EES runtime integration

New components under `SAPPub.Infrastructure/EES` provide:

- HTTP query execution against EES (`EESClient`) with retry behavior
- Metadata and data query orchestration (`EESAPIService`)
- Reflection-based mapping from `[EESInput]` attributes to EES query criteria
- Response flattening and model population via `EESTransformations`
- ID/code conversion through local lookup files (`LookupList_*.json`)

## 3) Metadata-driven mapping strategy

The DTOs (e.g., `KS2EnglandInformationDTO`) now define EES requirements at property level using:

- indicator ID
- time period
- filter(s)

This centralizes mapping in code rather than SQL/view configuration.

## 4) New generator project for versioned metadata classes

`SAPPub.EESCodeGenerator` is added to generate strongly typed constants for:

- `Identifiers`
- `Filters`

by querying EES metadata. The generated classes are versioned (`v1_x`) and used by DTO attributes.

## 5) Scope of the PR

Current implementation is **KS2-focused**. Other domains continue to rely on the existing pipeline + Postgres model.

## Comparison with existing documentation and previous ETL process

## Current documented model (before this PR)

Documentation currently describes a pipeline-centric architecture where:

- Data is ingested to Blob Storage, transformed, and loaded into Postgres (`docs/sap-data/*`)
- Postgres is the authoritative source (`docs/architecture/002-high-level-design.md`, `docs/sap-data/01-overview.md`)
- The web app reads from curated Postgres views

## What this PR changes relative to that model

For KS2 performance endpoints, this PR introduces a runtime API retrieval model that bypasses the ETL-to-Postgres path.

That means the implementation now partially diverges from docs that state Postgres is authoritative for application reads.

## Previous KS2 ETL/data flow

Previously, KS2 data was consumed from repository abstractions backed by JSON/Postgres (depending on environment) and ultimately aligned to ETL outputs and materialized views.

This PR replaces that for KS2 retrieval with direct EES calls and in-process transformation/mapping.

## Benefits highlighted by this PR

## 1) Direct EES API consumption

- Reduces dependency on batch ETL timing for KS2 freshness
- Reduces manual handling of KS2 CSV ingestion/mapping churn
- Keeps the retrieval logic closer to the source contract

## 2) Automatic metadata generation and version resilience

- The code generator pulls EES metadata and generates typed constants for filters/indicators
- DTO `EESInput` mappings reference generated constants rather than raw magic strings
- When EES versions/metadata change, regeneration provides a faster and safer update path with compile-time visibility

## 3) Potential to reduce Postgres dependency

If this approach is expanded consistently across all endpoints, the application could reduce or remove the need for Postgres as a runtime read dependency for profile/performance datasets.

Practical impact if fully adopted:

- Less operational overhead for DB provisioning, refresh, and ETL orchestration for these endpoints
- Fewer synchronization issues between source publication and DB availability
- Simpler local development for API-backed endpoints

## Gaps and considerations from review

- The architecture is currently hybrid (KS2 via EES API; most other areas via Postgres pipeline)
- Existing docs still describe Postgres-first reads and should be updated to reflect hybrid/transition state
- Some new paths use machine-specific absolute paths (e.g., lookup/generator output), which should be made configurable before broader rollout
- Test coverage for new EES integration and mapping behavior needs to be expanded

## What is next

## 1) Thorough testing

- Add unit tests for `EESTransformations`, `EESAPIService`, lookup resolution, and `KS2PerformanceEESRepository`
- Add integration tests with controlled EES API responses (success, partial data, retry/error paths)
- Add end-to-end/profile-page validation for KS2 user journeys in Razor Pages

## 2) Data verification and parity checks

- Build automated parity checks between current ETL/Postgres outputs and EES runtime outputs for KS2
- Verify identifier/filter/time-period mappings per DTO property
- Validate reason-code parsing and handling of missing/redacted values

## 3) Expand pattern across entities/endpoints

- Prioritize next entities by volatility and maintenance pain in ETL
- Replicate EES DTO + repository + mapping pattern in small, testable increments
- Keep fallback strategy during migration (feature flags or side-by-side comparison)

## 4) Documentation and architecture updates

- Update `docs/architecture/002-high-level-design.md` to describe hybrid data-access model
- Update `docs/sap-data/*` and related ADR notes to clarify where ETL remains required vs API-direct endpoints
- Add a developer guide for running metadata generation and refreshing `EESInput` mappings

## Conclusion

This PR is a strong foundational step toward API-first data access for KS2. It reduces manual data-mapping overhead, introduces version-aware metadata generation, and creates a path to reduce Postgres dependency if rolled out across all endpoints.

The immediate priority is to harden with tests, run formal data parity verification, and then scale the approach entity-by-entity with documentation updates in parallel.

## Summary for non-technical users (Project Management)

This change is a positive shift toward a simpler and faster way of delivering KS2 data.

In practical terms, the team is moving from a heavy process (download, transform, load into database) to a more direct process where the application can request data from the official source API when needed.

Key project-level benefits are:

- Faster updates to published data, with less waiting for full ETL cycles
- Lower operational overhead over time, by reducing dependency on database-heavy pipelines
- Less manual maintenance when dataset versions change, thanks to generated mappings
- Reduced delivery risk for future updates, because the mapping approach is more repeatable and less error-prone
- A clearer route to scaling this approach across other datasets and endpoints

If this model is expanded successfully, it can improve release agility, reduce support burden, and lower long-term maintenance cost while keeping data quality controls in place through testing and validation.
