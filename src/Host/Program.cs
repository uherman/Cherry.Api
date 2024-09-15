using Domain;
using Driven;
using Driving;
using Host;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

const string swaggerTitle = "Cherry API";
const string swaggerEndpoint = "/swagger/v1/swagger.json";

var ingressOptions = builder.Configuration.GetSection(IngressOptions.Section).Get<IngressOptions>();

builder.Services.AddDriving(builder.Configuration);
builder.Services.AddDriven(builder.Configuration);
builder.Services.AddDomain(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cherry API", Version = "v1" }));

var app = builder.Build();

if (builder.Environment.IsDevelopment())
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

app.MapGet("/", () => "Hello World!");

app.Run();