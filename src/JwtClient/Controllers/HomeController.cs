using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtClient.Controllers;

public class HomeController : Controller
{
	private readonly HttpClient _httpClient;

	public HomeController(IHttpClientFactory httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient();
	}

	public IActionResult Index()
	{
		return View();
	}

	[Authorize]
	public async Task<IActionResult> Secret()
	{
		var token = await HttpContext.GetTokenAsync("access_token");

		_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
		var httpResponseMessage = await _httpClient.GetAsync("https://localhost:18003/secret/index");

		var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();

		return View();
	}
}
