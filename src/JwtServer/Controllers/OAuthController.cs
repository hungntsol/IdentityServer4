using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtServer.Controllers;

public class OAuthController : Controller
{
	[HttpGet]
	public IActionResult Authorize(
		string response_type, // authorization flow
		string client_id, // client id
		string redirect_uri,
		string scope, // info client want to access (email, name, ...)
		string state) // random string to confirm that we are going to back the same client
	{
		var query = new QueryBuilder
		{
			{ "redirect_uri", redirect_uri },
			{ "state", state }
		};
		return View(model: query.ToString());
	}

	[HttpPost]
	public IActionResult Authorize(string username, string redirect_uri, string state)
	{
		const string code = "ASDFDS354THZZZXCV";

		var query = new QueryBuilder
		{
			{ "code", code },
			{ "state", state }
		};

		return Redirect($"{redirect_uri}{query}");
	}

	public async Task<IActionResult> Token(
		string grant_type,
		string code,
		string redirect_uri,
		string client_id)
	{
		// validate the code

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Email, "test@mail.com"),
			new(JwtRegisteredClaimNames.Sub, "12345")
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Secret));
		var token = new JwtSecurityToken(
			JwtConfig.Issuer,
			JwtConfig.Audience, claims,
			null,
			DateTime.Now.AddHours(12),
			new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

		var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

		var responseObject = new
		{
			access_token = accessToken,
			token_type = "Bearer"
		};

		return Json(responseObject);
	}

	[HttpGet]
	[Authorize]
	public IActionResult Validate()
	{
		if (HttpContext.Request.Query.TryGetValue("access_token", out var accessToken))
		{
			return Ok();
		}

		return BadRequest();
	}
}
