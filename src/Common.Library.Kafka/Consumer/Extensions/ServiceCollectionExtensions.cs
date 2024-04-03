using Common.Library.Kafka.Common.Extensions;
using Common.Library.Kafka.Consumer.Configuration;
using Common.Library.Kafka.Consumer.Interfaces;
using Common.Library.Kafka.Consumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace Common.Library.Kafka.Consumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumerHandler<TValueType, TOptions, THandlerType>(
        this IServiceCollection services,
        IConfiguration configuration,
        JsonSerializerSettings? serializerSettings = null)
        where TOptions : BaseConsumerKafkaOptions
        where THandlerType : class, IConsumerHandler<TValueType>
    {
        services.TryAddSingleton<IConsumerFactory, ConsumerFactory>();

        services.Configure<TOptions>(configuration.GetSection(typeof(TOptions).Name));

        services.AddJsonDeserializer<TValueType>(serializerSettings);

        services.TryAddSingleton<THandlerType>();

        services.AddHostedService<ConsumerBackgroundService<TValueType, TOptions, THandlerType>>();

        return services;
    }
}