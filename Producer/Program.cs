using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Contracts;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq();
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
