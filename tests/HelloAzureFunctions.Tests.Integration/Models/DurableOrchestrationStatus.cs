using Microsoft.DurableTask.Client;

namespace HelloAzureFunctions.Tests.Integration.Models;

internal class DurableOrchestrationStatus
{
	public string Name { get; set; }

	public string InstanceId { get; set; }

	public OrchestrationRuntimeStatus RuntimeStatus { get; set; }

	public DateTimeOffset CreatedAt { get; set; }

	public DateTimeOffset LastUpdatedAt { get; set; }

	public string SerializedInput { get; set; }

	public string SerializedOutput { get; set; }

	public object SerializedCustomStatus { get; set; }

	public object FailureDetails { get; set; }

	public bool IsRunning { get; set; }

	public bool IsCompleted { get; set; }
}
