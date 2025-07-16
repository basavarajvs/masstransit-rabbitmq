using Microsoft.Extensions.Hosting;
using MassTransit;
using Consumer;
using Microsoft.Extensions.Configuration;

await Host.CreateDefaultBuilder(args)
    .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory) // Set content root to the executable's directory
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ImageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqConfig = hostContext.Configuration.GetSection("RabbitMq");
                cfg.Host(rabbitMqConfig["Host"], "/", h =>
                {
                    h.Username(rabbitMqConfig["Username"]);
                    h.Password(rabbitMqConfig["Password"]);
                });

                cfg.ReceiveEndpoint("image-processing", e =>
                {
                    e.ConfigureConsumer<ImageConsumer>(context);
                });
            });
        });
    })
    .Build()
    .RunAsync();