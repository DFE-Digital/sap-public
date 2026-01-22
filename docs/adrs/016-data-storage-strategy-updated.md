# 016 - Architecture Decision Record: Data Storage Strategy 

* Status: accepted
* Deciders: Dan Murfitt, Gurpal Kahlon, Rajesh Gaddam
* Date: 11-2026 (November)

Technical Story: [description | ticket/issue URL] <!-- optional -->

## Context and Problem Statement

Concerns were raised about using the local json/csv store referenced in [ADR-004](004-data-storage-strategy.md), as such the decision was re-reviewed and the decision made to go straight to using postgres as database.

## Decision Drivers <!-- optional -->

* Concern on updating/deploying code every day
* Concern on ability of local json to be as performant as postgres


## Considered Options

* Use Postgres as database, as per previous ADR.


## Decision Outcome

* Use postgres, without testing assumptions on reliability. 

### Positive Consequences <!-- optional -->

* Keeps SAP Public and SAP Sector in sync
* Guaranteed database reliability

### Negative Consequences <!-- optional -->

* Additional overhead on development and deployment management

## Links <!-- optional -->

* [Previous Related ADR ADR-004](004-data-storage-strategy.md)