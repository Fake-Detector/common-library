using System.Text.Json;
using Confluent.Kafka;

namespace Common.Library.Kafka.Common.JsonSerializers;

internal sealed class JsonValueSerializer<T> : ISerializer<T>
{
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonValueSerializer(JsonSerializerOptions jsonSerializerOptions)
    {
        _serializerOptions = jsonSerializerOptions;
    }

    public byte[] Serialize(T data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data, _serializerOptions);
    }
}