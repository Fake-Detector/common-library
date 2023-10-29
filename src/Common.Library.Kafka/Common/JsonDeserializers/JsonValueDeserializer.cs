using System.Text.Json;
using Confluent.Kafka;

namespace Common.Library.Kafka.Common.JsonDeserializers;

internal sealed class JsonValueDeserializer<T> : IDeserializer<T>
{
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonValueDeserializer(JsonSerializerOptions jsonSerializerOptions)
    {
        _serializerOptions = jsonSerializerOptions;
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            throw new ArgumentNullException(nameof(data), "Null data encountered");
        
        var resultDeserialized = JsonSerializer.Deserialize<T>(data, _serializerOptions);
        
        if (resultDeserialized == null)
            throw new ConsumeException(new ConsumeResult<byte[], byte[]>
            {
                Message = new Message<byte[], byte[]>
                {
                    Value = data.ToArray()
                }
            }, new Error(ErrorCode.InvalidMsg), new ArgumentNullException(nameof(data), "Null data encountered"));

        return resultDeserialized;
    }
}