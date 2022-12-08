using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDbContext<AppDbContext>(dbContextOptionsBuilder =>
{
	dbContextOptionsBuilder.UseInMemoryDatabase("Identity-Mem");
});

services.AddIdentity<IdentityUser, IdentityRole>(identityOptions =>
	{
		identityOptions.Password.RequiredLength = 3;
		identityOptions.Password.RequireLowercase = false;
		identityOptions.Password.RequireNonAlphanumeric = false;
		identityOptions.Password.RequireUppercase = false;
		identityOptions.Password.RequireDigit = false;

		identityOptions.SignIn.RequireConfirmedEmail = true;
	})
	.AddDefaultTokenProviders()
	.AddEntityFrameworkStores<AppDbContext>();

services.ConfigureApplicationCookie(cookieAuthenticationOptions =>
{
	cookieAuthenticationOptions.Cookie.Name = "Identity";
	cookieAuthenticationOptions.LoginPath = "/Home/Login";
});

services.AddMailKit(optionsBuilder =>
{
	var opt = new MailKitOptions
	{
		Port = 25,
		SenderEmail = "no-reply@test.com",
		SenderName = "IdentityServer",
		Server = "localhost"
	};
	optionsBuilder.UseMailKit(opt);
});

services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapGet("/", () => "IdentityServer start");

app.Run();
