using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;

namespace IdentityServer.Controllers;

public class HomeController : Controller
{
	private readonly IEmailService _emailService;
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly UserManager<IdentityUser> _userManager;

	public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
		IEmailService emailService)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_emailService = emailService;
	}

	public IActionResult Index()
	{
		return View();
	}

	[Authorize]
	public IActionResult Secret()
	{
		return View();
	}

	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Login(string username, string password)
	{
		var user = await _userManager.FindByNameAsync(username);

		if (user is null)
		{
			return RedirectToAction("Register");
		}

		var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

		if (result.Succeeded)
		{
			return RedirectToAction("Index");
		}

		return View("Error");
	}

	public async Task<IActionResult> Logout()
	{
		await _signInManager.SignOutAsync();
		return View("Index");
	}

	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Register(string username, string password)
	{
		var user = new IdentityUser(username)
		{
			Email = username
		};

		var result = await _userManager.CreateAsync(user, password);

		if (result.Succeeded)
		{
			// verify email
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var link = Url.Action(nameof(EmailConfirm), "Home", new { userId = user.Id, code }, Request.Scheme,
				Request.Host.ToString());

			var html = $"<a href=\"{link}\">Verify account</a>";
			await _emailService.SendAsync(user.Email, "Verify", html, true);

			return RedirectToAction("EmailVerification");
		}

		return View("Error");
	}

	public IActionResult EmailVerification()
	{
		return View();
	}

	public async Task<IActionResult> EmailConfirm(string userid, string code)
	{
		var user = await _userManager.FindByIdAsync(userid);

		if (user is null)
		{
			return BadRequest();
		}

		var result = await _userManager.ConfirmEmailAsync(user, code);

		if (result.Succeeded)
		{
			return View();
		}

		return View("Error");
	}
}
