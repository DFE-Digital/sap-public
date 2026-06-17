# Testing Overview

## Goal
This project intends to automate all testing that can be automated.

## Test organization
There is a test project for each project in our solution. 

The test projects are located in a folder called `Tests` at the root of our solution.
The CI pipeline looks for projects in this folder and runs the tests in those projects.

## CI/CD integration
Our tests are run in the CI pipeline on every PR and on merging to the main branch. PRs cannot be merged if
any tests fail.

## Test coverage reporting
We are using coverlet for test coverage reporting. The coverage reports are generated in the CI pipeline 
and published on the information page of every PR.