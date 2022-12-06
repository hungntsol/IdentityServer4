using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
	swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
	swaggerGenOptions.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Type = SecuritySchemeType.OAuth2,
		Flows = new OpenApiOAuthFlows
		{
			ClientCredentials = new OpenApiOAuthFlow
			{
				TokenUrl = new Uri($"{builder.Configuration["Authentication:Authority"]}/connect/token"),
				Scopes = new Dictionary<string, string> { { "example.api", "API" } }
			}
		}
	});
	swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
			},
			new List<string> { "example.api" }
		}
	});
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(jwtBearerOptions =>
	{
		jwtBearerOptions.Authority = new Uri($"{builder.Configuration["Authentication:Authority"]}").AbsoluteUri;

		jwtBearerOptions.TokenValidationParameters.ValidateAudience = false;
		jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
	});

builder.Services.AddAuthorization(authorizationOptions =>
{
	authorizationOptions.AddPolicy("ApiScope",
		authorizationPolicyBuilder =>
		{
			authorizationPolicyBuilder.RequireAuthenticatedUser().RequireClaim("scope", "example.api");
		});
});

builder.Services.AddCors(corsOptions =>
{
	corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
	{
		corsPolicyBuilder.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
