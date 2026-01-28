# 012 - Tool For UI Testing

* **Status**: accepted

## Context and Problem Statement

What tool should be adopted within the SAP project to facilitate UI testing?

## Decision Drivers

* Within DfE’s Technical Guidance
* DfE projects using Cypress
	* [find-a-tuition-partner](https://github.com/DFE-Digital/find-a-tuition-partner)
	* [trams-data-api](https://github.com/DFE-Digital/trams-data-api)
* Tooling already built within SAP projects (Sector/Public)
  
## Considered Options

* [Cypress](https://cypress.io)
* Selenium / specflow
* Puppeteer
* [Playwright] (https://github.com/microsoft/playwright)

## Decision Outcome

As code has already been written using Playwright, SAP will continue to use it. 

Should a reason to chance arise then a new ADR will be written as is standard.