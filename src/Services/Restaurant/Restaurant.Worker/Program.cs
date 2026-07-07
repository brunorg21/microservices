using Messaging.Shared;
using Restaurant.Application;
using Restaurant.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();

builder.Services.AddRabbitMQ(builder.Configuration);

builder.Services.AddHostedService<RestaurantConsumer>();

var host = builder.Build();
host.Run();
