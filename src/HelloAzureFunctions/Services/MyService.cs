namespace HelloAzureFunctions.Services;

public class MyService : IMyService
{
	public string SayHello(string name) => $"Hello {name}!";
}
