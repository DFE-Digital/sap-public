# Security Overview

This document describes the security controls implemented in the SAP Public application.

The service follows the cross government Secure by Design approach, which means security is
built into delivery rather than added at the end. A mapping to the ten Secure by Design
principles is included at the end of this document.

## Context

- Most of the data the service holds is published open data about schools, so the main risks
  sit around identity, session handling and the integrity of ingested data
- The service runs in containers on Azure Kubernetes Service, with all infrastructure defined
  in Terraform

## Session and cookies

- `analytics_preference` does the user accept analytics cookies?
- `hide_banner` does the user wish to hide the cookie notification banner?
- `MySchoolsList` contains a list of school URNs for the user to compare against each other

- Google Analytics/Microsoft Clarity cookies as accepted/declined by the user

All the cookies on our service are listed on the public site `/Cookies/Preferences` and contain no PII. 

## Transport security

- HTTPS redirection is enabled
- PostgreSQL connections use SSL, set through `PGSSLMODE`

## Response headers

Implemented in `SAPPub.Web/Middleware/SecurityHeadersMiddleware.cs`.

- `X-Content-Type-Options` nosniff
- `X-Frame-Options` DENY
- `Referrer-Policy` origin-when-cross-origin


## Content Security Policy

The application uses a restrictive Content Security Policy (CSP) implemented in `SapPub.Web/Middleware/SecurityHeadersMiddleware.cs`:

- `default-src 'self'` restricts resources to the application origin by default.
- `base-uri 'self'` prevents unauthorised `<base>` elements.
- `frame-ancestors 'self'` restricts which sites can embed the application.
- `img-src` allows same-origin images, `data:` images, OpenStreetMap tiles, and Google Tag Manager.
- `style-src 'self'` allows same-origin stylesheets only.
- `font-src 'self' data:` allows same-origin fonts and embedded font data.
- `script-src` allows same-origin scripts, nonce-authorised inline scripts, Google Tag Manager,
  and Microsoft Clarity.
- `connect-src` allows same-origin requests and connections to approved analytics,
  postcode lookup, monitoring, and telemetry services.

In development environments, the policy also permits HTTP, HTTPS, WebSocket, and secure
WebSocket connections to any localhost port. This supports local development tools and
browser live-reload functionality.

This is joined with a 32 byte nonce generated per request. 

## Input and output handling

- Razor views encode output by default, protecting rendered establishment names and search
  terms drawn from external datasets
- Data access uses parameterised Dapper queries, so user supplied values are never concatenated
  into SQL
- Static file serving uses an explicit content type provider rather than inferring types

## Key and secrets management

- ASP.NET Core Data Protection is configured explicitly, with keys persisted outside the
  container so cookies stay valid across pod restarts and scale events
- `PostgresConnectionString` and `SentryDsn` are read from Azure Key Vault at deploy time
- No secret values are committed to the repository
- Secrets reach the application as Kubernetes secrets rather than as plain configuration
- GitHub Actions authenticates to Azure using federated OIDC credentials, so there is no long
  lived service principal secret in GitHub
- See `/docs/adrs/008-secrets-management.md` for the rationale

## Container and runtime hardening

Implemented in `Dockerfile`.

- Multi stage build, so the .NET SDK, the Node toolchain and the source tree are not present in
  the final image
- The container runs as a non root user through `APP_UID`
- Frontend dependencies install with `npm ci --ignore-scripts`, stopping package install scripts
  executing during the build
- Base images come from the official Microsoft container registry

## Change control

Implemented in `.github/workflows/`.

- Every change goes through a pull request with required approval and passing checks
- Review app environments are created by adding a `deploy` label and destroyed automatically
  when the label is removed or the pull request closes
- Deployment to test is automatic on merge to main. Production requires manual approval
- Health checks gate every deployment, with up to five retries before the deploy is failed
- Terraform changes are validated by `validate-infrastructure.yml`

## Logging and monitoring

- Serilog provides structured logging, configured per environment
- Sentry captures application errors, with `SendDefaultPii` set to false so personal data is not
  sent to a third party error tracker
- Logit is enabled through the AKS application module for centralised log aggregation
- Two health endpoints exist, `/healthcheck` for Kubernetes probes and `/health` for detailed
  diagnostics

## Assurance

- An IT Health Check has been carried out. All findings were remediated.
- An OWASP Top 10 assessment is in progress
- Security testing is described in `/docs/testing/007-security-tests.md`
- Secure coding rules for the team are in `/docs/developers/011-security.md`

## Mapping to the Secure by Design principles

- Principle 2, source secure technology products. Container hardening, supported framework
  versions, shared DfE modules, pipeline scanning
- Principle 4, design usable security controls. Web-Application Firewall, building based on least-requirement, 401 and 403 behaviour
- Principle 5, build in detect and respond security. Logging and monitoring, health checks
- Principle 6, design flexible architectures. Layered solution, single point configuration of
  headers and authentication, infrastructure as code
- Principle 7, minimise the attack surface. Deny by default authorisation, container hardening,
  single exposed port
- Principle 8, defend in depth. Transport security, response headers, CSP, cookies, data
  protection, secrets management
- Principle 9, embed continuous assurance. Test suites, pipeline scanning, IT Health Check
- Principle 10, make changes securely. Change control

Principles 1 and 3 are met through governance and risk activity rather than through code, and
are recorded separately.
