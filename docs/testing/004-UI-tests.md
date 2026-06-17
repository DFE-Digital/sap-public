# UI tests
We use UI tests to test the application up to the base repository layer which is faked.

We use this test layer to test page interactions and flows that are not easily tested in unit tests. 
This includes things like form submissions, navigation, and interactions with JavaScript components.

These tests are also useful for testing the data flow through the application and verifying that the 
different layers of the application are working together correctly and showing the correct data on the page.

These tests are located in the `UI` folder in the `SAPPub.Web.Tests` project. These tests spin up an in-memory
app and use Playwright to interact with the application and verify that it behaves correctly.
