using IdentityServer4.Models;

namespace IdentityServer;

public static class IdentityConfig
{
	public static IEnumerable<ApiScope> ApiScopes =>
		new List<ApiScope>
		{
			new("example.api", "APIv1")
		};

	public static IEnumerable<Client> Clients => new List<Client>
	{
		new()
		{
			ClientName = "Console Application",
			ClientId = "6941549A-5090-4C64-BE65-3A3D9059E6E3",
			AllowedGrantTypes = GrantTypes.ClientCredentials,
			ClientSecrets = new List<Secret> { new("secret".Sha256()) },
			AllowedScopes = new List<string> { "example.api" },
			AllowedCorsOrigins = new List<string> { "https://example.api:18001", "https://localhost:18001" }
		}
	};
}
