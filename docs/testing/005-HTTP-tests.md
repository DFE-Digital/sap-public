# HTTP and Integration Testing

HTTP and integration tests validate the application without browser automation.

## Scope

These tests verify:
- Controller endpoints return expected status codes
- Responses contain correct data
- Errors are handled safely

## Data access testing

The application uses Dapper with a PostgreSQL database.

Dapper queries are tested against a faked database to ensure:
- Queries execute successfully
- Parameters are applied correctly
- Paging and filtering behave as expected


