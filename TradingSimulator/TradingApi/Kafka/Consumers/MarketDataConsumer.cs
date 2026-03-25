using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Runtime;
using System.Text.Json;
using TradingApi.Contracts.MarketData;
using TradingApi.Kafka.Config;
using TradingApi.Kafka.Consumer;
using TradingApi.Services.Interfaces;

namespace TradingApi.Kafka.Consumers
{
    public class MarketDataConsumer : BackgroundService
    {
        private readonly MarketKafkaSettings _kafkaSettings;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MarketDataConsumer> _logger;

        public MarketDataConsumer(
            IOptions<MarketKafkaSettings> kafkaOptions,
            IServiceScopeFactory scopeFactory,
            ILogger<MarketDataConsumer> logger)
        {
            _kafkaSettings = kafkaOptions.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_kafkaSettings.Topic);
            _logger.LogInformation("Kafka config: {Bootstrap} {Topic} {Group}",
                _kafkaSettings.BootstrapServers,
                _kafkaSettings.Topic,
                _kafkaSettings.ConsumerGroupId);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    if (result?.Message?.Value == null)
                        continue;

                    var message = JsonSerializer.Deserialize<MarketDataMessage>(
                        result.Message.Value);

                    if (message != null)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var marketDataService = scope.ServiceProvider
                                .GetRequiredService<IMarketDataService>();

                            await marketDataService.BroadcastTickerUpdate(message);
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"[Kafka] Consume error: {ex.Error.Reason}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"[Kafka] Deserialization error: {ex.Message}");
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Kafka] Unexpected error: {ex.Message}");
                }
            }

            consumer.Close();
        }
    }
}