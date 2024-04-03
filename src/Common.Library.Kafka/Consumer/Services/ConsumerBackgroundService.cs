using Common.Library.Kafka.Consumer.Configuration;
using Common.Library.Kafka.Consumer.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Library.Kafka.Consumer.Services;

public class ConsumerBackgroundService<TValue, TOptions, THandlerType> : BackgroundService
    where TOptions : BaseConsumerKafkaOptions
    where THandlerType : class, IConsumerHandler<TValue>
{
    private readonly IConsumer<string, TValue> _consumer;
    private readonly IConsumerHandler<TValue> _consumerHandler;
    private readonly IOptionsMonitor<TOptions> _consumerOptions;
    private readonly ILogger<ConsumerBackgroundService<TValue, TOptions, THandlerType>> _logger;

    public ConsumerBackgroundService(
        IConsumerFactory consumerFactory,
        THandlerType consumerHandler,
        IDeserializer<TValue> deserializer,
        IOptionsMonitor<TOptions> consumerOptions,
        ILogger<ConsumerBackgroundService<TValue, TOptions, THandlerType>> logger)
    {
        _consumer = consumerFactory.CreateConsumer(consumerOptions.CurrentValue, deserializer);
        _consumerHandler = consumerHandler;
        _consumerOptions = consumerOptions;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        _logger.LogInformation("Start consumer handler: {Type} successful", typeof(TValue).FullName);

        var topics = _consumerOptions.CurrentValue.Topics
            .Split(",", StringSplitOptions.RemoveEmptyEntries).Select(it => it.Trim());

        _consumer.Subscribe(topics);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = _consumer.Consume(stoppingToken);

                await _consumerHandler.HandleMessage(message, stoppingToken);

                _consumer.StoreOffset(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occured: {Error}", e.Message);
            }
        }
    }
}