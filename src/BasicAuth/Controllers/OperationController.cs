using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuth.Controllers;

public class OperationController : Controller
{
    private readonly IAuthorizationService _authorizationService;

    public OperationController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<IActionResult> Open()
    {
        var cookieJar = new CookieJar(); // get resources from db, services
        var requirement = new OperationAuthorizationRequirement
        {
            Name = CookieJarOperations.Open
        };

        await _authorizationService.AuthorizeAsync(User, cookieJar, requirement);

        return View();
    }
}

public class CookieJarAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, CookieJar>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        CookieJar cookieJar)
    {
        switch (requirement.Name)
        {
            case CookieJarOperations.Look:
            {
                if (context.User.Identity!.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }

                break;
            }
            case CookieJarOperations.ComeNear:
            {
                if (context.User.HasClaim("level", "A"))
                {
                    context.Succeed(requirement);
                }

                break;
            }
        }

        return Task.CompletedTask;
    }
}

public static class CookieJarOperations
{
    public const string Open = "Open";
    public const string TakeCookie = "TakeCookie";
    public const string ComeNear = "ComeNear";
    public const string Look = "Look";
}

public class CookieJar
{
    public CookieJar()
    {
    }

    public CookieJar(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
