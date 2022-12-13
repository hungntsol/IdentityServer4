var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

const string jwtClientCookieSchema = "JwtClientCookie";
const string authServerSchema = "OAuthServer";
services.AddAuthentication(authenticationOptions =>
	{
		// check the cookie schema whether user is authenticated
		authenticationOptions.DefaultAuthenticateScheme = jwtClientCookieSchema;
		// deal with a cookie when sign in
		authenticationOptions.DefaultSignInScheme = jwtClientCookieSchema;
		// check if user is allow to to something
		authenticationOptions.DefaultChallengeScheme = authServerSchema;
	})
	.AddCookie(jwtClientCookieSchema)
	.AddOAuth(authServerSchema, oAuthOptions =>
	{
		oAuthOptions.CallbackPath = "/oauth/callback";
		oAuthOptions.ClientId = "client_id";
		oAuthOptions.ClientSecret = "client_secret";
		oAuthOptions.AuthorizationEndpoint = "https://localhost:18003/oauth/authorize";
		oAuthOptions.TokenEndpoint = "https://localhost:18003/oauth/token";
	});

services.AddControllersWithViews();

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

app.MapDefaultControllerRoute();
app.MapGet("/", () => "JwtClient start");
app.Run();
