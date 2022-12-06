// See https://aka.ms/new-console-template for more information

using IdentityModel.Client;

Console.WriteLine("Hello, World!");

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:18000");

if (disco.IsError)
{
	Console.WriteLine(disco.Error);
	return;
}

var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
	Address = disco.TokenEndpoint,
	ClientId = "6941549A-5090-4C64-BE65-3A3D9059E6E3",
	ClientSecret = "secret",
	Scope = "example.api"
});

if (tokenResponse.IsError)
{
	Console.WriteLine(tokenResponse.Error);
}

Console.WriteLine(tokenResponse.Json);

Console.ReadKey();
