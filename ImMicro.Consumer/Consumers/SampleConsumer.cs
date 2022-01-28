using ImMicro.Contract.Consumer;
using MassTransit;

namespace ImMicro.Consumer.Consumers;

public class SampleConsumer : IConsumer<SampleConsumerCommand>
{
    public Task Consume(ConsumeContext<SampleConsumerCommand> context)
    {
        //Sample consumer
        Console.WriteLine($"Sample Consumer {DateTime.UtcNow}");

        return Task.CompletedTask;
    }
}