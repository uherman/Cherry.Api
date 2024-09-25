using Domain;
using Driven;
using Driving;
using Host;
using Host.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

const string swaggerTitle = "Cherry API";
const string swaggerEndpoint = "/swagger/v1/swagger.json";

var ingressOptions = builder.Configuration.GetSection(IngressOptions.Section).Get<IngressOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Auth0Options.Configure(builder.Configuration));

builder.Services.AddAuthorizationBuilder()
    .AddDefaultPolicy("Default", policy => policy.RequireAuthenticatedUser());

builder.Services.AddDriving(builder.Configuration);
builder.Services.AddDriven(builder.Configuration);
builder.Services.AddDomain(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

if (builder.Environment.IsEnvironment("NoProxy"))
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint(swaggerEndpoint, swaggerTitle));
}
else
{
    app.UseSwagger(c => c.PreSerializeFilters.Add((swaggerDoc, _) =>
    {
        swaggerDoc.Servers = [new OpenApiServer { Url = ingressOptions.Uri }];
    }));

    app.UseSwaggerUI(options => options.SwaggerEndpoint($"{ingressOptions.Uri}{swaggerEndpoint}", swaggerTitle));
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();