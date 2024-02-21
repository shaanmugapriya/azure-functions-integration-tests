using HelloAzureFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace HelloAzureFunctions;

public class HelloOrchestrator(IMyService myService)
{
	[Function(nameof(HelloOrchestrator))]
	public async Task<List<string>> Run([OrchestrationTrigger] TaskOrchestrationContext context)
	{
		ILogger logger = context.CreateReplaySafeLogger(nameof(HelloOrchestrator));
		logger.LogInformation("Saying hello.");
		var outputs = new List<string>
		{
			await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"),
			await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"),
			await context.CallActivityAsync<string>(nameof(SayHello), "London")
		};

		return outputs;
	}

	[Function(nameof(SayHello))]
	public string SayHello([ActivityTrigger] string name)
	{
		return myService.SayHello(name);
	}
}