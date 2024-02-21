using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace HelloAzureFunctions;

public class HttpStart
{
	[Function(nameof(HttpStart))]
	public async Task<HttpResponseData> Run(
		[HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
		[DurableClient] DurableTaskClient client,
		FunctionContext executionContext)
	{
		ILogger logger = executionContext.GetLogger(nameof(HttpStart));

		string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(HelloOrchestrator));

		logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

		return client.CreateCheckStatusResponse(req, instanceId);
	}
}