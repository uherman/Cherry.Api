using Domain;
using Driven;
using Driving;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDriving(builder.Configuration);
builder.Services.AddDriven(builder.Configuration);
builder.Services.AddDomain(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Run();