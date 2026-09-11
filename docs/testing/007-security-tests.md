# Security Testing

Security testing ensures the service protects data and enforces access boundaries.

Security testing is treated as a distinct activity from functional testing. It combines
automated checks in CI, targeted manual checks when sensitive areas change, and independent
assessment carried out by specialists outside the delivery team.

## Automated security testing

The following security checks run in CI today.

- Snyk. Dependency and container image vulnerability scanning, run as part of the docker image
  build step in `build-and-deploy.yml` using the `SNYK_TOKEN` secret. It detects known
  vulnerabilities in open source packages and in the base image layers.


## Independent assessment

- An IT Health Check has been carried out against the service. All findings raised were
  remediated and no issues have been reported since
- An OWASP Top 10 assessment is currently in progress. The outcome will be recorded here once
  it completes

## Dependabot updates

Snyk picks up the dotnet issues and vulnerabilities, Dependabot can also pick up the JS/NPM-sourced issues and raises a notification of issue or attempts to fix and raise a PR to fix.