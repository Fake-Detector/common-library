using Confluent.Kafka;

namespace Common.Library.Kafka.Consumer.Configuration;

public abstract class BaseConsumerKafkaOptions
{
    public string Topics { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Latest;
    public bool EnableAutoCommit { get; set; } = true;
    public bool EnableAutoOffsetStore { get; set; } = false;
}