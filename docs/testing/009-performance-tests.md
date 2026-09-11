# Performance Testing

Performance testing ensures the service remains responsive as data and usage grow.

## Application performance

Metrics monitored include:
- p50 and p95 response times
- Requests per second
- Error and exception rates
- CPU and memory usage per pod

## Database performance

The application uses PostgreSQL and is read-heavy. Data is populated by a separate data-load engine.

We monitor:
- Cpu and Memory
- Database availability
- Storage
- DB connections
- Throughput

## Production monitoring

Performance in production is monitored continuously.
Only smoke tests are run in production; no load testing is performed there.
