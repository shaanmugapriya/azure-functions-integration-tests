using Polly;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HelloAzureFunctions.Tests.Integration.Fixtures;

public class AzureFunctionFixture : IDisposable
{
	private readonly string _path = Directory.GetCurrentDirectory();
	private readonly string _testOutputPath = Path.Combine(Directory.GetCurrentDirectory(), "integration-test-output.log");
	private readonly int _port = 7071;
	private readonly string _baseUrl;
	private readonly Process _process;

	public readonly HttpClient HttpClient;

	public AzureFunctionFixture()
	{
		_baseUrl = $"http://localhost:{_port}";

		HttpClient = new HttpClient()
		{
			BaseAddress = new Uri(_baseUrl)
		};

		if (File.Exists(_testOutputPath))
		{
			File.Delete(_testOutputPath);
		}

		DirectoryInfo directoryInfo = new(_path);
		_process = StartProcess(_port, directoryInfo);
		_process.OutputDataReceived += (sender, args) =>
		{
			File.AppendAllLines(_testOutputPath, [args.Data]);
		};
		_process.BeginOutputReadLine();
	}

	public void Dispose()
	{
		if (!_process.HasExited)
		{
			_process.Kill(entireProcessTree: true);
		}

		_process.Dispose();
		HttpClient.Dispose();
	}

	public async Task WaitUntilFunctionsAreRunning()
	{
		PolicyResult<HttpResponseMessage> result =
			await Policy.TimeoutAsync(TimeSpan.FromSeconds(30))
				.WrapAsync(Policy.Handle<Exception>().WaitAndRetryForeverAsync(index => TimeSpan.FromMilliseconds(500)))
				.ExecuteAndCaptureAsync(() => HttpClient.GetAsync(""));

		if (result.Outcome != OutcomeType.Successful)
		{
			throw new InvalidOperationException("The Azure Functions project doesn't seem to be running.");
		}
	}

	private static Process StartProcess(int port, DirectoryInfo workingDirectory)
	{
		string fileName = "func";
		string arguments = $"start --port {port} --verbose";

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			fileName = "powershell.exe";
			arguments = $"func start --port {port} --verbose";
		}

		ProcessStartInfo processStartInfo = new(fileName, arguments)
		{
			UseShellExecute = false,
			CreateNoWindow = true,
			RedirectStandardOutput = true,
			WorkingDirectory = workingDirectory.FullName,
			EnvironmentVariables =
			{ 
				// Passing an additional environment variable to the application,
                // So it can control the behavior when running for Integration Tests
				[ApplicationConstants.IsRunningIntegrationTests] = "true"
			}
		};

		Process process = new() { StartInfo = processStartInfo };
		process.Start();

		return process;
	}
}