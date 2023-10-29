using Confluent.Kafka;

namespace Common.Library.Kafka.Common.Configuration;

public class CommonKafkaOptions
{
    public ConsumerOptions ConsumerOptions { get; set; }
 
    public string BrokerHost { get; set; }
}

public class ConsumerOptions
{
    public string Topics { get; set; }
    public string GroupId { get; set; }
    public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Latest;
    public bool EnableAutoCommit { get; set; } = true;
    public bool EnableAutoOffsetStore { get; set; } = false;
}

