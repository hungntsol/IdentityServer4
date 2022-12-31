using JwtApi.JwtRequirements;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication();

builder.Services.AddAuthorization(authorizationOptions =>
{
	authorizationOptions.DefaultPolicy = new AuthorizationPolicyBuilder()
		.AddRequirements(new JwtRequirement())
		.Build();
});

builder.Services.AddHttpClient()
	.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthorizationHandler, JwtRequirementHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
