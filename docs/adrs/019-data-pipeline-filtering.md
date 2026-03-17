# 019 - Data Pipeline Filtering

* Status: accepted
* Deciders: Dan Murfitt, Cath Lawlor, Rajesh Gaddam, Naomi Todd
* Date: 2026-03-09

## Context and Problem Statement

Data needs to be updated in the pipeline to filter out certain school types, schools closed > 3 academic years and other business rules for the service to follow including the gradual inclusion of each key stage of data.

## Decision Drivers <!-- optional -->

* Speed of pipeline
* Need to keep all filter rules in one place for easy updating & transparency

## Considered Options

* Filtering when csv files ingested in data pipeline so only relevant data is stored in db
* Filtering in code when querying data
* Filtering when building views in data pipeline

## Decision Outcome

* Filtering when building views in data pipeline - with hardcoded sql filters but in only 1 location. 
* Views are used to access data by web app.
* Can be updated to be configurable in future if needed.

### Positive Consequences <!-- optional -->

*	Data Integrity: retain the raw, unfiltered data, which is valuable for auditing, reprocessing, or future requirements.
*	Flexibility: Filtering logic can be changed or extended without re-importing data.
*	Performance: postgres efficiently handles filtering with materialized views.
*   Data that shouldn't be on the service won't be exposed

### Negative Consequences <!-- optional -->

* 	Slightly more storage is used.
