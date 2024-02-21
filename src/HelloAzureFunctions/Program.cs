using HelloAzureFunctions;
using HelloAzureFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = new HostBuilder()
	.ConfigureFunctionsWorkerDefaults()
	.ConfigureAppConfiguration((context, builder) =>
	{
		builder.AddEnvironmentVariables();
	})
	.ConfigureServices(services =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();

		IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

		// TODO: If required, register specific services when running integration tests
		if (configuration.GetValue<bool>(ApplicationConstants.IsRunningIntegrationTests))
		{
			services.AddSingleton<IMyService, MyTestService>();
		}
		else
		{
			services.AddSingleton<IMyService, MyService>();
		}
	})
	.Build();

host.Run();
