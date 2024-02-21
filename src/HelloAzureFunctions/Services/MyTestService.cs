namespace HelloAzureFunctions.Services;

public class MyTestService : IMyService
{
	public string SayHello(string name) => $"From Test: Hello {name}!";
}