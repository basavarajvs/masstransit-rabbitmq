using Microsoft.Extensions.Hosting;
using MassTransit;
using Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqConfig = hostContext.Configuration.GetSection("RabbitMq");
                cfg.Host(rabbitMqConfig["Host"], "/", h =>
                {
                    h.Username(rabbitMqConfig["Username"]);
                    h.Password(rabbitMqConfig["Password"]);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build()
    .RunAsync();


class Worker : BackgroundService
{
    private readonly IBus _bus;

    public Worker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:image-processing"));
        await endpoint.Send(new ImageToProcess { ImagePath = "/path/to/image.jpg" }, stoppingToken);

        Console.WriteLine("Sent image processing request");
    }
}