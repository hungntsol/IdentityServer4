using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuth.Controllers;

public class HomeController : Controller
{
    private readonly IAuthorizationService _authorizationService;

    public HomeController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize("v1_secret")]
    public IActionResult Secret()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Authenticate()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, "test@mail.com"),
            new(ClaimTypes.NameIdentifier, "123"),
            new("type", "v1")
        };

        var identityClaim = new ClaimsIdentity(claims, "Identity Claim");

        var userPrincipal = new ClaimsPrincipal(identityClaim);

        HttpContext.SignInAsync(userPrincipal);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DoStuff(
        [FromServices] IAuthorizationService authorizationService)
    {
        var builder = new AuthorizationPolicyBuilder("Schema");
        var policy = builder.RequireClaim(ClaimTypes.Email).Build();

        var authResult = await authorizationService.AuthorizeAsync(User, policy);

        if (authResult.Succeeded)
        {
        }

        return RedirectToAction("Index");
    }
}
