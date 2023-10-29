using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Library.Kafka.Common.Configuration;
using Common.Library.Kafka.Common.Extensions;
using Common.Library.Kafka.Common.JsonSerializers;
using Common.Library.Kafka.Producer.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Library.Kafka.Producer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProducerHandler<T>(
        this IServiceCollection services,
        JsonSerializerOptions? serializerOptions = null)
    {
        services.AddJsonSerializer<T>(serializerOptions);
        
        services.AddSingleton<IProducer<string, T>>(provider =>
        {
            var kafkaOptions = provider.GetRequiredService<IOptionsSnapshot<CommonKafkaOptions>>().Value;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = kafkaOptions.BrokerHost,
                Acks = Acks.All
            };
            
            var serializer = provider.GetRequiredService<ISerializer<T>>();

            return new ProducerBuilder<string, T>(producerConfig)
                .SetValueSerializer(serializer)
                .Build();
        });

        services.AddSingleton<IProducerHandler<T>, ProducerHandler<T>>();

        return services;
    }
}