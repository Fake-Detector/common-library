using Common.Library.Kafka.Common.Configuration;
using Common.Library.Kafka.Consumer.Configuration;
using Common.Library.Kafka.Consumer.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Common.Library.Kafka.Consumer.Services;

internal class ConsumerFactory : IConsumerFactory
{
    private readonly IOptionsMonitor<CommonKafkaOptions> _kafkaOptions;

    public ConsumerFactory(IOptionsMonitor<CommonKafkaOptions> kafkaOptions) => _kafkaOptions = kafkaOptions;

    public IConsumer<string, TValueType> CreateConsumer<TValueType, TOptions>(
        TOptions consumerOptions,
        IDeserializer<TValueType> deserializer)
        where TOptions : BaseConsumerKafkaOptions =>
        new ConsumerBuilder<string, TValueType>(new ConsumerConfig
            {
                BootstrapServers = _kafkaOptions.CurrentValue.BrokerHost,
                GroupId = consumerOptions.GroupId,
                AutoOffsetReset = consumerOptions.AutoOffsetReset,
                EnableAutoCommit = consumerOptions.EnableAutoCommit,
                EnableAutoOffsetStore = consumerOptions.EnableAutoOffsetStore
            })
            .SetValueDeserializer(deserializer)
            .Build();
}