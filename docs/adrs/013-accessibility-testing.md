# 0013 - Accessibility Testing

ADR taken from https://github.com/DFE-Digital/sts-plan-technology-for-your-school

**Status**: Accepted  
**Deciders**: Dan Murfitt  
**Date**: 2025-10-15

## Context and Problem Statement

How do we best test the website for accessibility issues?

## Decision Drivers

* Ease of testing
* Reliability
* Existing DFE usage

## Considered Options

### Manual Testing

We could manually test the website, using tools such as [axe](https://www.deque.com/axe/), which is recommended by the Government & DFE.

### Automation Testing

We could automate the website testing in our end-to-end tests.

SAP Sector has scripting to run automated accessibility tests as part of their [Integration tests](https://github.com/DFE-Digital/sap-sector/tree/main/Tests/SAPSec.UI.Tests/AccessibilityTests)

We could also use the [A11y Github Action](https://github.com/marketplace/actions/web-accessibility-evaluation) to review one of each page and report back any issues. 

## Decision Outcome

* We will manually test each component when built, using browser tools like Axe/Wave. 
* We will eventually run the A11y github action (above) to check each page as part of the UI tests. 
    * [Work not started](https://trello.com/c/0lffhrED/121-automated-testing) but in story breakdown.

