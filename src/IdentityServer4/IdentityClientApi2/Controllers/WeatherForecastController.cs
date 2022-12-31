using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace IdentityClientApi2.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
	private static readonly string[] Summaries =
	{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

	private readonly IHttpClientFactory _httpClientFactory;

	private readonly ILogger<WeatherForecastController> _logger;

	public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
	{
		_logger = logger;
		_httpClientFactory = httpClientFactory;
	}

	[HttpGet(Name = "GetWeatherForecast")]
	public IEnumerable<WeatherForecast> Get()
	{
		return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
	}

	[HttpGet("Identity")]
	public async Task<IActionResult> Identity()
	{
		var client = _httpClientFactory.CreateClient("IdentityServer");
		var discoveryDoc = await client.GetDiscoveryDocumentAsync();

		var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
		{
			Address = discoveryDoc.TokenEndpoint,
			ClientId = "E85DBCAB-AE07-4FBA-B000-E7C26A03458A",
			ClientSecret = "secret",
			Scope = "api1"
		});

		var client1 = "https://localhost:7188";
		var apiClient = _httpClientFactory.CreateClient();
		apiClient.SetBearerToken(tokenResponse.AccessToken);

		var response = await apiClient.GetAsync(client1 + "/secret");

		return Ok(new
		{
			access_token = tokenResponse.AccessToken,
			mesage = await response.Content.ReadAsStringAsync()
		});
	}
}
