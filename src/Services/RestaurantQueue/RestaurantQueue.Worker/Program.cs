using Messaging.Shared;
using RestaurantQueue.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddRabbitMQ(builder.Configuration);

var host = builder.Build();
host.Run();
