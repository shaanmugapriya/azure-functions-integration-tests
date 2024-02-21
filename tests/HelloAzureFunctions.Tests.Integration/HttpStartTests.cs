using FluentAssertions;
using HelloAzureFunctions.Tests.Integration.Constants;
using HelloAzureFunctions.Tests.Integration.Extensions;
using HelloAzureFunctions.Tests.Integration.Fixtures;
using HelloAzureFunctions.Tests.Integration.Helpers;
using HelloAzureFunctions.Tests.Integration.Models;
using Microsoft.DurableTask.Client;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace HelloAzureFunctions.Tests.Integration;

[Collection(TestCollectionNames.AzureFunctionCollection)]
public class HttpStartTests(AzureFunctionFixture azureFunctionFixture)
{
	private readonly AzureFunctionFixture _azureFunctionFixture = azureFunctionFixture;

	[Fact]
	public async Task HttpStart_Returns_CorrectResult()
	{
		HttpResponseMessage response = await AzureFunctionFixtureHelper.HttpStart(_azureFunctionFixture, null);

		response.StatusCode.Should().Be(HttpStatusCode.Accepted);

		HttpManagementPayload httpManagementPayload = await response.Content.ReadFromJsonAsync<HttpManagementPayload>();
		StatusQueryResult statusQueryResult = new()
		{
			RuntimeStatus = OrchestrationRuntimeStatus.Running
		};

		using HttpClient client = new();

		while (statusQueryResult.RuntimeStatus == OrchestrationRuntimeStatus.Running
			|| statusQueryResult.RuntimeStatus == OrchestrationRuntimeStatus.Pending)
		{
			await Task.Delay(500);

			response = await client.GetAsync(httpManagementPayload.StatusQueryGetUri);
			if (!response.IsSuccessStatusCode)
			{
				break;
			}

			statusQueryResult = await response.ToStatusQueryResult();
		}

		statusQueryResult.RuntimeStatus.Should().Be(OrchestrationRuntimeStatus.Completed);

		JsonNode expectedOutput = JsonNode.Parse($$"""
			[
				"From Test: Hello Tokyo!",
				"From Test: Hello Seattle!",
				"From Test: Hello London!"
			]
		""");

		JsonAssertionHelper.BeEquivalentTo(expectedOutput, statusQueryResult.Output);
	}
}