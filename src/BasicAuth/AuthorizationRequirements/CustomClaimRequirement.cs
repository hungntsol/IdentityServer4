using Microsoft.AspNetCore.Authorization;

namespace BasicAuth.AuthorizationRequirements;

public class CustomClaimRequirement : IAuthorizationRequirement
{
	public CustomClaimRequirement(string claimType)
	{
		ClaimType = claimType;
	}

	public string ClaimType { get; }
}

public class CustomClaimRequirementHandler : AuthorizationHandler<CustomClaimRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
		CustomClaimRequirement requirement)
	{
		var hasClaimType = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

		if (hasClaimType)
		{
			context.Succeed(requirement);
		}

		return Task.CompletedTask;
	}
}
