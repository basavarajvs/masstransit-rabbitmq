using Microsoft.Extensions.Hosting;
using MassTransit;
using Consumer;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ImageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ReceiveEndpoint("image-processing", e =>
                {
                    e.ConfigureConsumer<ImageConsumer>(context);
                });
            });
        });
    })
    .Build()
    .RunAsync();
