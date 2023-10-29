using Common.Library.Kafka.Consumer.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.Library.Kafka.Consumer.Services;

public class ConsumerBackgroundService<T> : BackgroundService
{
    private readonly IConsumer<string, T> _consumer;
    private readonly IConsumerHandler<T> _consumerHandler;
    private readonly ILogger<ConsumerBackgroundService<T>> _logger;

    public ConsumerBackgroundService(
        IConsumer<string, T> consumer,
        IConsumerHandler<T> consumerHandler,
        ILogger<ConsumerBackgroundService<T>> logger)
    {
        _consumer = consumer;
        _consumerHandler = consumerHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = _consumer.Consume(stoppingToken);
                
                await _consumerHandler.HandleMessage(message, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occured: {Error}", e.Message);
            }
        }
    }
}