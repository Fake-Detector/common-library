using Common.Library.Kafka.Common.Configuration;
using Common.Library.Kafka.Common.JsonDeserializers;
using Common.Library.Kafka.Common.JsonSerializers;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.Library.Kafka.Common.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddJsonSerializer<T>(
        this IServiceCollection services,
        JsonSerializerSettings? serializerOptions)
    {
        services.TryAddSingleton<ISerializer<T>>(_ =>
        {
            serializerOptions ??= new JsonSerializerSettings
            {
                Converters = { new StringEnumConverter() }
            };

            return new JsonValueSerializer<T>(serializerOptions);
        });

        return services;
    }

    internal static IServiceCollection AddJsonDeserializer<T>(
        this IServiceCollection services,
        JsonSerializerSettings? serializerOptions)
    {
        services.TryAddSingleton<IDeserializer<T>>(_ =>
        {
            serializerOptions ??= new JsonSerializerSettings
            {
                Converters = { new StringEnumConverter() }
            };

            return new JsonValueDeserializer<T>(serializerOptions);
        });

        return services;
    }

    public static IServiceCollection AddCommonKafka(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CommonKafkaOptions>(config.GetSection(nameof(CommonKafkaOptions)));
        
        return services;
    }
}