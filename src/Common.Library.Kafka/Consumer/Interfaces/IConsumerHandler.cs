using Confluent.Kafka;

namespace Common.Library.Kafka.Consumer.Interfaces;

public interface IConsumerHandler<T>
{
    public Task HandleMessage(ConsumeResult<string, T> message, CancellationToken cancellationToken);
}