using HelloAzureFunctions.Tests.Integration.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelloAzureFunctions.Tests.Integration.Extensions;

internal static class HttpResponseMessageExtensions
{
	static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
	{
		PropertyNameCaseInsensitive = true,
		Converters = { new JsonStringEnumConverter() }
	};

	public static async Task<StatusQueryResult> ToStatusQueryResult(this HttpResponseMessage httpResponseMessage)
	{
		var content = await httpResponseMessage.Content.ReadAsStringAsync();

		StatusQueryResult statusQueryResult = JsonSerializer.Deserialize<StatusQueryResult>(content, _jsonSerializerOptions);

		return statusQueryResult;
	}
}
