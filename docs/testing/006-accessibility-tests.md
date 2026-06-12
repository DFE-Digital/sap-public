# Accessibility Tests
We automate accessibility tests using Playwright's accessibility testing capabilities. These tests are located 
in the AccessibilityTests.cs file in the `UI` folder in the `SAPPub.Web.Tests` project. These tests spin up an
in-memory app and use the AxeCore extension for Playwright to interact with the application and verify that it behaves correctly from an accessibility 
standpoint.

These tests are run in the CI pipeline and the results are published on the information page of every PR. 

Critical errors should be considered as test failures and fixed before merge.
Moderate and low errors are added to the product backlog and prioritised for fix. 
