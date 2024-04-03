using Confluent.Kafka;
using Newtonsoft.Json;

namespace Common.Library.Kafka.Common.JsonDeserializers;

internal sealed class JsonValueDeserializer<T> : IDeserializer<T>
{
    private readonly JsonSerializerSettings _serializerOptions;

    public JsonValueDeserializer(JsonSerializerSettings jsonSerializerOptions)
    {
        _serializerOptions = jsonSerializerOptions;
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            throw new ArgumentNullException(nameof(data), "Null data encountered");

        var resultDeserialized = JsonConvert.DeserializeObject<T>(
            Deserializers.Utf8.Deserialize(data, false, SerializationContext.Empty),
            _serializerOptions);

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