using Messaging.Shared;
using Restaurant.Infra;
using Restaurant.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfra(builder.Configuration);

builder.Services.AddRabbitMQ(builder.Configuration);

builder.Services.AddHostedService<RestaurantConsumer>();

var host = builder.Build();
host.Run();
