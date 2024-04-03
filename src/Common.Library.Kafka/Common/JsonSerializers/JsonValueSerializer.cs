using Confluent.Kafka;
using Newtonsoft.Json;

namespace Common.Library.Kafka.Common.JsonSerializers;

internal sealed class JsonValueSerializer<T> : ISerializer<T>
{
    private readonly JsonSerializerSettings _serializerOptions;

    public JsonValueSerializer(JsonSerializerSettings jsonSerializerOptions) =>
        _serializerOptions = jsonSerializerOptions;

    public byte[] Serialize(T data, SerializationContext context) =>
        Serializers.Utf8.Serialize(JsonConvert.SerializeObject(data, _serializerOptions), SerializationContext.Empty);
}