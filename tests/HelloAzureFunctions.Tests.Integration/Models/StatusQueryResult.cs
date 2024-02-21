using Microsoft.DurableTask.Client;
using System.Text.Json.Nodes;

namespace HelloAzureFunctions.Tests.Integration.Models;

internal class StatusQueryResult
{
	public string Name { get; set; }

	public string InstanceId { get; set; }

	public OrchestrationRuntimeStatus RuntimeStatus { get; set; }

	public JsonNode Input { get; set; }

	public object CustomStatus { get; set; }

	public JsonNode Output { get; set; }

	public DateTimeOffset CreatedTime { get; set; }

	public DateTimeOffset LastUpdatedTime { get; set; }

	public object HistoryEvents { get; set; }

	public string Error { get; set; }
}
