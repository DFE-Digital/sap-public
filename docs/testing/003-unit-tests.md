# Unit testing

We are following a full unit testing strategy.
This means that we are writing unit tests for as much as our code as possible, and we are running those tests as part of our continuous integration pipeline.

## Tools
We use 
- xUnit as our unit testing framework
- Moq as our mocking library
- Anglesharp for parsing our html
- Bogus for generating fake data

## Testing the views

We have a set of tests that test the views. These tests are located in the `Unit` folder in the
`SAPPub.Web.Tests` project. These tests mock the service used by the controller action, spin up an in-memory app and 
test that the page renders correctly. We use Anglesharp to parse the html and verify that the correct elements are present.
