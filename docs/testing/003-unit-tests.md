# Unit Testing

Unit tests cover the internal logic of the web application.

## Scope

Unit tests focus on:
- ViewModel mapping
- Aggregation and calculation logic
- Filtering and grouping rules
- Defensive handling of missing or partial data

## What is not unit tested

The following are not covered by unit tests:
- GOV.UK frontend rendering
- Styling and layout

## Data access

Dapper query construction logic may be unit tested where practical, but SQL correctness and performance are validated using integration tests.

## Tooling

- xUnit
- Mocking only where required to avoid over-mocking

Unit tests run in CI and must pass before changes can be merged.