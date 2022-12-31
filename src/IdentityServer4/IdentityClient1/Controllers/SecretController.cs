using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityClient1.Controllers;

public class SecretController : Controller
{
	[Route("/secret")]
	[Authorize]
	public string Index()
	{
		return "Client1 secret";
	}
}
