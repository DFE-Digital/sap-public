# 022 - Establishment Caching Further

**Status**: proposed
**Deciders**: Dan Murfitt
**Date**: 2026-08-19


## Context and Problem Statement

Implementing the Establishment cache could be further improved by caching *all* the establishments on load of the application/pod. 

A cache clearing option may also need to be implemented, as the current only way to clear the cache is by deployment. This is not necessarily a problem currently due to the number of releases we are doing, however longer-term this could become an issue with daily GIAS updated.

## Decision Drivers


## Proposal

On application startup, load the list of establishments in to memory. 

Enable either a URL to clear the cache (and make sure it hits all pods some how) or maybe a Github pipeline to update all pods. -- Reviewing needed on options.

## Decision Outcome
