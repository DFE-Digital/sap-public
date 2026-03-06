# 016 - Postcode lookup 

**Status**: accepted
**Deciders**: Dan Murfitt, Catherine Lawlor
**Date**: 03-2026 (March)

## Context and Problem Statement

Implementing the search for schools by postcode necessitated the ability to convert a postcode to a geo locationi (either lat/lon or northing/easting)

## Considered Options

* The [DfE architecture docs](https://github.com/DFE-Digital/architecture/blob/master/common-components/index.md#postcode) provide a number of postcode and geolocation options.


## Decision Outcome

* postcode.io was chosen as the postcode lookup service. It is a free and open source service that provides a simple API for converting postcodes to geolocations.
