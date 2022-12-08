var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication("CookieBasicAuth")
	.AddCookie("CookieBasicAuth",
		cookieAuthenticationOptions =>
		{
			cookieAuthenticationOptions.Cookie.Name = "IdentityServer.BasicAuth";
			cookieAuthenticationOptions.LoginPath = "/Home/Authenticate";
		});

builder.Services.AddAuthorization(authorizationOptions =>
{
	authorizationOptions.AddPolicy("v1_secret",
		authorizationPolicyBuilder =>
		{
			authorizationPolicyBuilder.RequireAuthenticatedUser().RequireClaim("type", "v3");
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Map("/", () => "IdentityServer start");

app.Run();
