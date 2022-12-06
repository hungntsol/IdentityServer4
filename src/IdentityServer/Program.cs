using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddIdentityServer()
	.AddDeveloperSigningCredential()
	.AddInMemoryApiScopes(IdentityConfig.ApiScopes)
	.AddInMemoryClients(IdentityConfig.Clients);

var app = builder.Build();

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
