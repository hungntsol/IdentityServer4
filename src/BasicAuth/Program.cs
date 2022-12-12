using System.Security.Claims;
using BasicAuth.AuthorizationRequirements;
using BasicAuth.Controllers;
using BasicAuth.Transformers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddAuthentication("CookieBasicAuth")
    .AddCookie("CookieBasicAuth",
        cookieAuthenticationOptions =>
        {
            cookieAuthenticationOptions.Cookie.Name = "Identity.BasicAuth";
            cookieAuthenticationOptions.LoginPath = "/Home/Authenticate";
        });

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy("v1_secret",
        authorizationPolicyBuilder =>
        {
            authorizationPolicyBuilder.RequireAuthenticatedUser()
                .AddRequirements(new CustomClaimRequirement(ClaimTypes.Email));
        });
});

builder.Services.AddScoped<IAuthorizationHandler, CustomClaimRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
builder.Services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

builder.Services.AddControllersWithViews();

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

//app.MapRazorPages();
app.Map("/", () => "BasicAuth start");
app.MapDefaultControllerRoute();

app.Run();
