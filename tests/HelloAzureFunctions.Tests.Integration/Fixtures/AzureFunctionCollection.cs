using HelloAzureFunctions.Tests.Integration.Constants;

namespace HelloAzureFunctions.Tests.Integration.Fixtures;

[CollectionDefinition(TestCollectionNames.AzureFunctionCollection)]
public class AzureFunctionCollection : ICollectionFixture<AzureFunctionFixture>
{
}
