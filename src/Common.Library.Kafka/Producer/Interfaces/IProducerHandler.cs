namespace Common.Library.Kafka.Producer.Interfaces;

public interface IProducerHandler<in T>
{
    public Task Produce(string topic, T message, CancellationToken cancellationToken);
}