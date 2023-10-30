using System.Text.Json;
using Common.Library.Kafka.Common.Configuration;
using Common.Library.Kafka.Common.Extensions;
using Common.Library.Kafka.Consumer.Interfaces;
using Common.Library.Kafka.Consumer.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Library.Kafka.Consumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumerHandler<TValueType, THandlerType>(
        this IServiceCollection services,
        JsonSerializerOptions? serializerOptions = null)
        where THandlerType : class, IConsumerHandler<TValueType>
    {
        services.AddJsonDeserializer<TValueType>(serializerOptions);
        
        services.AddSingleton<IConsumer<string, TValueType>>(provider =>
            {
                var kafkaOptions = provider.GetRequiredService<IOptionsMonitor<CommonKafkaOptions>>().CurrentValue;
                var consumerOptions = kafkaOptions.ConsumerOptions;

                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = kafkaOptions.BrokerHost,
                    GroupId = consumerOptions.GroupId,
                    AutoOffsetReset = consumerOptions.AutoOffsetReset,
                    EnableAutoCommit = consumerOptions.EnableAutoCommit,
                    EnableAutoOffsetStore = consumerOptions.EnableAutoOffsetStore
                };

                var deserializer = provider.GetRequiredService<IDeserializer<TValueType>>();

                return new ConsumerBuilder<string, TValueType>(consumerConfig)
                    .SetValueDeserializer(deserializer)
                    .Build();
            }
        );

        services.AddSingleton<IConsumerHandler<TValueType>, THandlerType>();

        services.AddHostedService<ConsumerBackgroundService<TValueType>>();

        return services;
    }
}