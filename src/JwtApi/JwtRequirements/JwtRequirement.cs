using Microsoft.AspNetCore.Authorization;

namespace JwtApi.JwtRequirements;

public class JwtRequirement : IAuthorizationRequirement
{
}

public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
{
	private readonly HttpClient _httpClient;
	private readonly HttpContext _httpContext;

	public JwtRequirementHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
	{
		_httpClient = httpClientFactory.CreateClient();
		_httpContext = httpContextAccessor.HttpContext!;
	}


	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
		JwtRequirement requirement)
	{
		if (_httpContext.Request.Headers.TryGetValue("Authorization", out var oauthHeader))
		{
			var accessToken = oauthHeader.ToString().Split(' ')[1];

			var httpResponseMessage =
				await _httpClient.GetAsync($"https://localhost:18003/OAuth/Validate/?access_token={accessToken}");

			if (httpResponseMessage.IsSuccessStatusCode)
			{
				context.Succeed(requirement);
			}
		}
	}
}
