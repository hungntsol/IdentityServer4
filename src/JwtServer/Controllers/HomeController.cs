using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtServer.Controllers;

public class HomeController : Controller
{
    private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Secret()
    {
        return View();
    }

    public IActionResult Authenticate()
    {
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

        return Ok(new { AccessToken = _jwtSecurityTokenHandler.WriteToken(token) });
    }
}
