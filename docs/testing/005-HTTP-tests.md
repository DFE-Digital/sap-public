# HTTP and Integration Testing

HTTP and integration tests validate the application without browser automation.

## Scope

These tests verify:
- Controller endpoints return expected status codes
- Responses contain correct data
- Errors are handled safely

## Data access testing

The application uses Dapper with a PostgreSQL database.

Currently we are building an End-to-end testing process for the whole pipeline to ensure data access is accurate and legitimate. 


