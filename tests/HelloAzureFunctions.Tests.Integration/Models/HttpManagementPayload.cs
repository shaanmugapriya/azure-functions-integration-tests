using System.Text.Json.Serialization;

namespace HelloAzureFunctions.Tests.Integration.Models;

internal class HttpManagementPayload
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("purgeHistoryDeleteUri")]
	public string PurgeHistoryDeleteUri { get; set; }

	[JsonPropertyName("sendEventPostUri")]
	public string SendEventPostUri { get; set; }

	[JsonPropertyName("statusQueryGetUri")]
	public string StatusQueryGetUri { get; set; }

	[JsonPropertyName("terminatePostUri")]
	public string TerminatePostUri { get; set; }
}
