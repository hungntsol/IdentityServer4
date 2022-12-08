using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Home;

public class AuthenticateModel : PageModel
{
	public void OnGet()
	{
		var v1Claim = new List<Claim>
		{
			new(ClaimTypes.Email, "test@mail.com"),
			new(ClaimTypes.NameIdentifier, "123"),
			new("type", "v1")
		};

		var v2Claim = new List<Claim>
		{
			new("type", "v2"),
			new(ClaimTypes.Name, "Test")
		};

		var identity = new ClaimsIdentity(v1Claim, "IdentityServer Claims");
		var license = new ClaimsIdentity(v2Claim, "License Claims");

		var userPrincipal = new ClaimsPrincipal(new[] { identity, license });

		HttpContext.SignInAsync(userPrincipal);

		RedirectToPage("/Home/Secret");
	}
}
