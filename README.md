# Integration tests for Azure Functions

The basic concept is as follows (Note: this project uses XUnit.net).

1. Create a [fixture class](tests/HelloAzureFunctions.Tests.Integration/Fixtures/AzureFunctionFixture.cs) that implements `IDisposable` and on the constructor, the Function Application to test is spun up using `func start`. And doing the cleanup on `Dispose()`.

2. Create an XUnit Collection Fixture using the above fixture, so the single test context (the function application) will get shared among different tests in several test classes, and it will get cleaned up after all the tests in the test classes have finished.

3. Integration tests can be executed locally as well as in a pipeline (Windows or Linux). Refer the [pipeline](.github/workflows/azure-functions-app-dotnet.yml).
