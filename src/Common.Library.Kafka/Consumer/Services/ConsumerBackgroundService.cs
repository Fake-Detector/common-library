using Common.Library.Kafka.Common.Configuration;
using Common.Library.Kafka.Consumer.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Library.Kafka.Consumer.Services;

internal class ConsumerBackgroundService<T> : BackgroundService
{
    private readonly IConsumer<string, T> _consumer;
    private readonly IOptionsMonitor<CommonKafkaOptions> _kafkaOptions;
    private readonly IConsumerHandler<T> _consumerHandler;
    private readonly ILogger<ConsumerBackgroundService<T>> _logger;

    public ConsumerBackgroundService(
        IConsumer<string, T> consumer,
        IOptionsMonitor<CommonKafkaOptions> kafkaOptions,
        IConsumerHandler<T> consumerHandler,
        ILogger<ConsumerBackgroundService<T>> logger)
    {
        _consumer = consumer;
        _kafkaOptions = kafkaOptions;
        _consumerHandler = consumerHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start consumer handler: {Type} successful", typeof(T).FullName);

        var topics = _kafkaOptions.CurrentValue.ConsumerOptions.Topics
            .Split(",", StringSplitOptions.RemoveEmptyEntries).Select(it => it.Trim());

        _consumer.Subscribe(topics);

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