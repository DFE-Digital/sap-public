# Testing Strategy Overview

This document describes the testing strategy for the SAP Sector web application.

The service is a read-only, data-driven web application built using:
- ASP.NET Core MVC (.NET)
- Dapper for data access

The application supports the following user journeys:

A member of the public can 
- view the profile of a school, viewing performance, absence, attendance measures, as well as basic overview information for the school. 
- search for a school by name of the school
- search for a school with abbreviations or other part-queries and be presented with a list of possible matches (saint -> st etc)
- search for schools in an area, by postcode, or town

The main areas of the service are:
1. View school profile
2. Find/search for schools

The aim of this strategy is to provide confidence that the service:
- Works correctly for all journeys
- Presents accurate and complete data
- Scales reliably under expected usage
- Meets security, accessibility, and performance expectations

## Testing principles

- Automation first
- Focus on real user journeys
- Shift-left testing in CI
- Minimal but meaningful UI automation
- Non-functional testing is planned and repeatable

## Testing pyramid

The service follows a testing pyramid approach:
- Unit tests for business logic and data shaping
- Integration and HTTP tests for controllers and data access
- End-to-end UI tests for critical journeys
- Separate non-functional testing for security, performance, load, and accessibility

## Environments

Testing is performed across:
- Local development
- CI pipelines
- Review apps
- Test environment
- Production (monitoring and smoke checks only)

## Test execution and ownership

This document describes **what** types of testing are performed.

For clarity on **where, when, and by whom** each type of test is executed (including CI, deployment, release, routine testing, external audits, manual testing, DevOps/IaC testing, and load/performance testing), see:

- **`010-test-execution-model.md`**

This provides a lifecycle-based view of test execution and ownership across the service.
