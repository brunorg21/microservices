using Auth.Api.Application;
using Auth.Api.Infra;
using Auth.Api.Infra.Cache;
using Messaging.Shared;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Setup Serilog

Log.Logger = new LoggerConfiguration()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

//Add Infra
builder.Services.AddInfra(builder.Configuration);

builder.Services.AddRabbitMQ(builder.Configuration);

//Add Application
builder.Services.AddApplication();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

await app.AddCacheHealthCheck();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api-docs");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
