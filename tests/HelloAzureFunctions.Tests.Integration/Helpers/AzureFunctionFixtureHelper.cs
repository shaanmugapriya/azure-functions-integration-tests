using HelloAzureFunctions.Tests.Integration.Fixtures;

namespace HelloAzureFunctions.Tests.Integration.Helpers;

internal class AzureFunctionFixtureHelper
{
	public static async Task<HttpResponseMessage> HttpStart(AzureFunctionFixture azureFunctionFixture, StringContent payload)
	{
		await azureFunctionFixture.WaitUntilFunctionsAreRunning();

		HttpResponseMessage response = await azureFunctionFixture.HttpClient.PostAsync("api/HttpStart", payload);

		return response;
	}
}