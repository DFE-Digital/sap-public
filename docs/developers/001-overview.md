# Overview for Developers

This document gives developers a **working understanding** of the service.

It intentionally stays high-level and practical.
For architectural rationale, see `/docs/architecture` and `/docs/adrs`.

---

## What this service does

This service allows the public to:
- search for schools
- compare a cherry-picked list of schools
- view detailed school information about performance, as well as general content around school admissions and attendance policies

It is a public-facing ASP.NET Core MVC application.

---

## Technology summary

- ASP.NET Core MVC (.NET)
- PostgreSQL for data storage
- PostgreSQL Full Test Search for school searching
- Postcodes.io for Postcode searching
- GOV.UK / DfE Frontend / MoJ Frontend
- Playwright for end-to-end testing

---

## How the code is organised (at a glance)

- **Web**: controllers, views, UI concerns
- **Core**: domain logic and application services
- **Infrastructure**: database, search, external integrations

Controllers → Services → Repositories / Search

---

## Where to find deeper detail

- Architecture diagrams & decisions: `/docs/architecture`
- Architecture Decision Records: `/docs/adrs`
