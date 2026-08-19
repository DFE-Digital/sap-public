# 021 - Establishment Caching

**Status**: accepted
**Deciders**: Dan Murfitt
**Date**: 2026-08-19


## Context and Problem Statement

During Load testing, (and subsequent profiling) it was identified that a large number of requests were being made of the Establishment entity

- DfE Analytics Phase - would query URN and return IsKS2, IsKS4, IsKS5. 
- PrimaryPhaseValidator - would query URN and return IsKS2
- Page Models - Would query URN and return School Name, and LA number/name for display on pages

As a minimum these three calls could potentially double the number of calls made before a page can be loaded or invalidated. 

## Decision Drivers

- We don't currently know the expected capacity of the service, but it this feels like a "quick-win" to cut down the number of unnecessary calls. This is because the load test results indicated the postgres DB as a limitation, AKS pods were at no more than 50% load. 
- We could rework *at least* the first two uses (above) into a single method, but this would still be one call per page which could be eliminated entirely with the proposal. 

## Proposal

We implement an in-memory cache of the basic Establishment information needed 

- URN
- Establishment Name
- LAId
- LA Name
- IsKS2
- IsKS4
- IsKS5

Further fields might be added in future. 

The largest this would be (exported data into CSV format) would be ~3MB, when the overall server memory is running around 200MB, this would be of little to no consequence. 

This would be done the first time the establishment is called by a user and then kept indefinitely. [ADR-022](016-establishment-caching-further.md) covers some extra options.

## Decision Outcome

Build the cache method and review. 