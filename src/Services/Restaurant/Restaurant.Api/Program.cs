using Restaurant.Api.Application;
using Restaurant.Infra;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add infra

builder.Services.AddInfra(builder.Configuration);

// Add Application

builder.Services.AddApplication();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

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
