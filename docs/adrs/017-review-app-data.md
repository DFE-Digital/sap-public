# 017 - Review App data source

* Status: accepted
* Deciders: Dan Murfitt, Stu Sherwin, Cath Lawlor, Gurpal Kahlon, Rajesh Gaddam, Mahmood Mahmood, Vipin Reddy 
* Date: 2026-02-04

## Context and Problem Statement

When originally designed, an assumption was made that when Review Apps (raised by PRs and tagged as Deploy) would be able to use Test databases so that the full pipeline isn't needed to be run each time. This could be a ~10 minute lead time on the already lengthy process. The discussion as to options to solve this was then had.

## Decision Drivers <!-- optional -->

* Speed of pipeline
* School Digital allowed processes and security architecture

## Considered Options

* Use existing github actions to take a backup from Test and deploy when environment created
* Store SQL scripts locally from the last checked-in run, each holding their data and schema
* Build data seeding functionality to provide a set of test cases

## Decision Outcome

Option chosen is first in the considered option list - restore a backup whenever the review environment is queued. 

This decision will be reviewed once the MVP has been able to be delivered with a plan to move towards data seeding. 

### Positive Consequences <!-- optional -->

* Speed of delivery, this is already being done by other teams in SD
* Data is automatically kept up to date from other processes

### Negative Consequences <!-- optional -->

* Changing the schema may become difficult when the pipeline is automatically instantiating it on each run. 
* Data is production-level, so contains a lot of data but much of it repetitive.

## Links <!-- optional -->

* Link to ticket for review post-MVP delivery.