using Confluent.Kafka;

namespace Common.Library.Kafka.Common.Configuration;

internal class CommonKafkaOptions
{
    public ConsumerOptions ConsumerOptions { get; set; }

    public string BrokerHost { get; set; }
}

internal class ConsumerOptions
{
    public string Topics { get; set; }
    public string GroupId { get; set; }
    public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Latest;
    public bool EnableAutoCommit { get; set; } = true;
    public bool EnableAutoOffsetStore { get; set; } = false;
}