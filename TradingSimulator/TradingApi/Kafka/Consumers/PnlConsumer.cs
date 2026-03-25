using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TradingApi.Contracts.Pnl;
using TradingApi.Hubs;
using TradingApi.Kafka.Config;
using Confluent.Kafka;


namespace TradingApi.Kafka.Consumer
{

    public class PnlConsumer : BackgroundService
    {
        private readonly IHubContext<PnlHub> _hubContext;
        private readonly KafkaSettings _kafkaSettings;
        private readonly ILogger<PnlConsumer> _logger;

        public PnlConsumer(
            IHubContext<PnlHub> hubContext,
            IOptions<PnlKafkaSettings> settings,
            ILogger<PnlConsumer> logger)
        {
            _hubContext = hubContext;
            _kafkaSettings = settings.Value;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumerLoop(stoppingToken), stoppingToken);
        }

        private void ConsumerLoop(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(_kafkaSettings.Topic);

            _logger.LogInformation("Kafka config: {Bootstrap} {Topic} {Group}",
                _kafkaSettings.BootstrapServers,
                _kafkaSettings.Topic,
                _kafkaSettings.ConsumerGroupId);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);

                    if (result?.Message?.Value == null)
                        continue;

                    var update = JsonSerializer.Deserialize<PnlUpdate>(result.Message.Value);

                    if (update == null)
                        continue;

                    _ = _hubContext.Clients
                        .Group(update.User)
                        .SendAsync("PnLUpdate", update, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka consumer stopping...");
            }
            finally
            {
                consumer.Close();
            }
        }
    }

}
