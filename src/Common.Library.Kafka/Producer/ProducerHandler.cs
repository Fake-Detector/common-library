using Common.Library.Kafka.Producer.Interfaces;
using Common.Library.Kafka.Exceptions;
using Confluent.Kafka;

namespace Common.Library.Kafka.Producer;

internal class ProducerHandler<T> : IProducerHandler<T>
{
    private readonly IProducer<string, T> _producer;

    public ProducerHandler(IProducer<string, T> producer) => _producer = producer;

    public async Task Produce(string topic, T message, CancellationToken cancellationToken)
    {
        var messageToProduce = new Message<string, T>
        {
            Key = DateTime.UtcNow.ToLongDateString(),
            Value = message
        };

        var result = await _producer.ProduceAsync(topic, messageToProduce, cancellationToken);

        if (result.Status != PersistenceStatus.Persisted) throw new ProduceKafkaException("Message not persisted");
    }
}