using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer.Config;

public static class IdentityConfig
{
	public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
	{
		new("example.api.v1", "Example Api v1")
		{
			Scopes = { "api1" }
		},
		new("example.api.v2", "Example Api v2")
	};

	public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
	{
		new("api1")
	};

	public static IEnumerable<Client> Clients => new List<Client>
	{
		new()
		{
			ClientId = "E85DBCAB-AE07-4FBA-B000-E7C26A03458A",
			ClientSecrets = new List<Secret> { new("secret".ToSha256()) },
			AllowedGrantTypes = GrantTypes.ClientCredentials,
			AllowedScopes = { "api1" }
		}
	};
}
