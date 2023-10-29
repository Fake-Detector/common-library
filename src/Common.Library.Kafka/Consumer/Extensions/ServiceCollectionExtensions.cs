using Common.Library.Kafka.Common.Configuration;
using Common.Library.Kafka.Consumer.Interfaces;
using Common.Library.Kafka.Consumer.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Library.Kafka.Consumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumerHandler<TValueType, THandlerType>(this IServiceCollection services)
        where THandlerType : class, IConsumerHandler<TValueType>
    {
        services.AddSingleton<IConsumer<string, TValueType>>(provider =>
            {
                var kafkaOptions = provider.GetRequiredService<IOptionsSnapshot<CommonKafkaOptions>>().Value;
                var consumerOptions = kafkaOptions.ConsumerOptions;

                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = kafkaOptions.BrokerHost,
                    GroupId = consumerOptions.GroupId,
                    AutoOffsetReset = consumerOptions.AutoOffsetReset,
                    EnableAutoCommit = consumerOptions.EnableAutoCommit,
                    EnableAutoOffsetStore = consumerOptions.EnableAutoOffsetStore
                };

                return new ConsumerBuilder<string, TValueType>(consumerConfig).Build();
            }
        );

        services.AddSingleton<IConsumerHandler<TValueType>, THandlerType>();

        services.AddHostedService<ConsumerBackgroundService<TValueType>>();

        return services;
    }
}