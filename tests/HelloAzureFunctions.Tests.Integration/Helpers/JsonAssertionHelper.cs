using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace HelloAzureFunctions.Tests.Integration.Helpers;

internal class JsonAssertionHelper
{
	public static void BeEquivalentTo(JsonNode expected, JsonNode actual)
	{
		var expectedJToken = JToken.Parse(expected.ToJsonString());
		var actualJToken = JToken.Parse(actual.ToJsonString());

		actualJToken.Should().BeEquivalentTo(expectedJToken);
	}
}
