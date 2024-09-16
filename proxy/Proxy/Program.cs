using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Proxy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var authOptions = builder.Configuration.GetSection(Auth0Options.Section).Get<Auth0Options>();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = authOptions.Domain;
    options.ClientId = authOptions.ClientId;
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapReverseProxy();

// TODO: Remove this when FE is deployed
app.MapGet("/", () =>
{
    app.Logger.LogInformation("GET / {Timestamp} ", DateTimeOffset.UtcNow);

    return Results.Json(new
    {
        Message = "Cherry reverse proxy is running",
        Timestamp = DateTimeOffset.UtcNow
    });
});

app.MapGet("Account/Login", (HttpContext context, [FromQuery] string returnUrl = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri(returnUrl)
        .Build();

    return context.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("Account/Logout", async context =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
        .WithRedirectUri("/")
        .Build();

    await context.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.Run();