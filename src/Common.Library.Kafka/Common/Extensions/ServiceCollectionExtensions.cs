using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Library.Kafka.Common.Configuration;
using Common.Library.Kafka.Common.JsonDeserializers;
using Common.Library.Kafka.Common.JsonSerializers;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Library.Kafka.Common.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddJsonSerializer<T>(
        this IServiceCollection services, 
        JsonSerializerOptions? serializerOptions)
    {
        services.AddSingleton<ISerializer<T>, JsonValueSerializer<T>>(_ =>
        {
            serializerOptions ??= new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            return new JsonValueSerializer<T>(serializerOptions);
        });

        return services;
    }
    
    internal static IServiceCollection AddJsonDeserializer<T>(
        this IServiceCollection services, 
        JsonSerializerOptions? serializerOptions)
    {
        services.AddSingleton<IDeserializer<T>, JsonValueDeserializer<T>>(_ =>
        {
            serializerOptions ??= new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            return new JsonValueDeserializer<T>(serializerOptions);
        });

        return services;
    }

    public static IServiceCollection AddCommonKafka(IServiceCollection services, IConfiguration config)
    {
        services.Configure<CommonKafkaOptions>(config.GetSection(nameof(CommonKafkaOptions)));
        
        
        return services;
    }
    
}