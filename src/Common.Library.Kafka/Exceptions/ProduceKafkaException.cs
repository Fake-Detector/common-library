namespace Common.Library.Kafka.Exceptions;

public class ProduceKafkaException : Exception
{
    public ProduceKafkaException() : base()
    {
    }
    
    public ProduceKafkaException(string message) : base(message)
    {
    }
}