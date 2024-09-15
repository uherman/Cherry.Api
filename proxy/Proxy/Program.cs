var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

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

app.Run();