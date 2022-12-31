using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtServer.Controllers;

public class SecretController : Controller
{
	[Authorize]
	public IActionResult Index()
	{
		return Ok("JwtServer Secret message");
	}
}
