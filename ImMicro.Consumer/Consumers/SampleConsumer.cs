using ImMicro.Contract.Consumer;
using MassTransit;

namespace ImMicro.Consumer.Consumers;

public class SampleConsumer : IConsumer<SampleConsumerCommand>
{
    public Task Consume(ConsumeContext<SampleConsumerCommand> context)
    {
        Console.WriteLine($"Sample Consumer - RequestTime {context.Message.RequestTime}");

        return Task.CompletedTask;
    }
}