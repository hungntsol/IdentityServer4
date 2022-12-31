using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAuthentication("JwtBearer")
	.AddJwtBearer("JwtBearer", jwtBearerOptions =>
	{
		const string server = "https://localhost:15000";
		jwtBearerOptions.Authority = server;
		jwtBearerOptions.Audience = "example.api.v1";
		jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
		jwtBearerOptions.TokenValidationParameters.ValidateAudience = true;
		jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = false;
	});

builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
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

app.MapControllers();

app.Run();
