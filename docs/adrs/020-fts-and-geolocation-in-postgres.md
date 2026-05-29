## 020 - FTS and Geolocation in Postgres

- **Status:** Accepted
- **Deciders:** Dan Murfitt, Cath Lawlor, Rajesh Gaddam, Naomi Todd
- **Date:** 2026-03-16

---

### Context and Problem Statement

- Existing text search implemented using Lucene (in-memory provider)
- Lucene index is built during web application startup
- Startup index build introduces: 
    - Increased application startup time
    - Delays in deployments (e.g. ~30 minutes for review app refresh)
    - Operational complexity in CI/CD pipeline
- In-memory index not ideal for: 
    - High traffic scenarios
    - Horizontal scaling (multiple pods require separate index instances)
- Lucene was originally selected before Postgres was introduced into the architecture
- System now has a Postgres database available as the primary data store
- Maintaining a separate search technology adds unnecessary complexity
- Updating search data requires rebuilding Lucene indexes, adding further overhead

---

### Decision Drivers

- Reduce application startup time and deployment delays
- Simplify architecture by removing separate search infrastructure
- Centralise search/indexing to a single source of truth (database)
- Improve scalability across multiple application instances/pods
- Reduce memory footprint in the application layer
- Enable easier and incremental updates to searchable data
- Align with existing Postgres-based architecture and tooling
- Maintain equivalent search functionality (school name and postcode search)
- Minimise operational and maintenance overhead

---

### Considered Options

#### Option 1: Continue with Lucene (current implementation)

- **Pros:** 
    - Proven fast text search performance
    - Already implemented
- **Cons:** 
    - Startup index build delays
    - Deployment pipeline complexity
    - Poor fit for distributed/multi-pod environments
    - Higher memory usage in application
    - Difficult to keep index in sync with data updates

#### Option 2: Move to Postgres Full-Text Search (FTS) and geospatial features

- **Pros:** 
    - Uses built-in database capabilities
    - No separate indexing system required in the app
    - Indexing handled at data layer (via pipeline)
    - Centralised and consistent across all services
    - Easier to update incrementally with data changes
    - Removes startup indexing cost
    - Supports both text search and postcode/geospatial queries (with extensions like PostGIS)
- **Cons:** 
    - Requires implementation effort and migration
    - Performance compared to Lucene assumed acceptable but not extensively benchmarked

---

### Decision Outcome

- Replace Lucene-based search with Postgres full-text search and geospatial querying
- Move index creation and maintenance into the data pipeline
- Remove Lucene index build from application startup
- Implement equivalent behaviour for: 
    - School name text search
    - Postcode-based search queries

---

### Positive Consequences

- Faster application startup (no index build step)
- Reduced deployment times and improved developer experience
- Simplified architecture (fewer moving parts)
- Centralised search index accessible by all application instances
- Better scalability in distributed environments (multi-pod setups)
- Lower memory usage in application layer
- Easier data updates without full index rebuilds
- Alignment with existing Postgres-based system design
- Simplification of test setup and maintenance (no separate Lucene index management)

---

### Negative Consequences

- Potential risk that Postgres search performance may not match Lucene in all cases
- Additional effort required to implement and validate Postgres-based search
- Need to reimplement postcode/geospatial search logic & add non-generic search specific methods to EstablishmentRepository
- Dependency on database performance for search responsiveness
- Loss of some advanced Lucene-specific capabilities (if not replicated)