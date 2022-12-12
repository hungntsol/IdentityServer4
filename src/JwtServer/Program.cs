using System.Text;
using JwtServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddControllersWithViews();

services.AddAuthentication("OAuth")
    .AddJwtBearer("OAuth", jwtBearerOptions =>
    {
        jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
        jwtBearerOptions.TokenValidationParameters.ValidateAudience = true;
        jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;

        jwtBearerOptions.TokenValidationParameters.ValidIssuer = JwtConfig.Issuer;
        jwtBearerOptions.TokenValidationParameters.ValidAudience = JwtConfig.Audience;
        jwtBearerOptions.TokenValidationParameters.IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Secret));

        // events
        jwtBearerOptions.Events = new JwtBearerEvents
        {
            OnMessageReceived = messageReceivedContext =>
            {
                if (messageReceivedContext.Request.Query.ContainsKey("access_token"))
                {
                    messageReceivedContext.Token = messageReceivedContext.Request.Query["access_token"];
                }

                return Task.CompletedTask;
            }
        };
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

app.MapDefaultControllerRoute();

app.MapGet("/", () => "JwtServer start");

app.Run();
