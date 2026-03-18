using Customer.Api.Application;
using Customer.Api.Infra;
using Customer.Api.Infra.Cache;
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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
