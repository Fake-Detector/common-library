using Common.Library.Kafka.Consumer.Configuration;
using Confluent.Kafka;

namespace Common.Library.Kafka.Consumer.Interfaces;

public interface IConsumerFactory
{
    IConsumer<string, TValueType> CreateConsumer<TValueType, TOptions>(
        TOptions consumerOptions,
        IDeserializer<TValueType> deserializer)
        where TOptions : BaseConsumerKafkaOptions;
}