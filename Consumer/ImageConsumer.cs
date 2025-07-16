using MassTransit;
using System.Threading.Tasks;
using Contracts;

namespace Consumer
{
    public class ImageConsumer : IConsumer<ImageToProcess>
    {
        public Task Consume(ConsumeContext<ImageToProcess> context)
        {
            System.Console.WriteLine($"Received image to process: {context.Message.ImagePath}");
            return Task.CompletedTask;
        }
    }
}
