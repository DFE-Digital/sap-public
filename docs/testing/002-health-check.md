# Health checks
We have two health check endpoints in our application. One is located at `/health` and the other is located at 
`/healthcheck`.

These endpoints confirm:
- The application has started successfully
- Required dependencies are reachable
- The service is ready to receive traffic

## Deployment checks

Health endpoints are used by:
- AKS liveness and readiness probes
- Deployment validation
- Autoscaling decisions

A deployment is considered successful only once the readiness check passes.
