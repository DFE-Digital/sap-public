# UI Testing (Playwright)

*STATE - GRANDFATHERED BUT NOT REVIEWED*

UI tests validate the end-to-end behaviour of the service from a user perspective.

## Tooling

Playwright is used for UI automation because it is:
- Reliable for modern web applications
- Fast in headless CI environments
- Well suited to accessibility checks

## Scope

UI automation focuses only on critical journeys.

### School user journey
- Search by name completes
- Search by partial name returns list of near neighbours
- Search by postcode returns list of nearby schools
- Search by town returns list of nearby schools
- Each profile page returns OK status
- Seeded data is rendered correctly
- Charts render correctly

## Execution

UI tests run:
- Against review apps for pull requests
- Against test environments on merge to main
- Nightly for full regression coverage