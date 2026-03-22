using Confluent.Kafka;
using PnlEngine.Models;
using PnlEngine.Services;
using System.Text.Json;

namespace PnlEngine.Consumers
{
    internal class EquityPriceConsumer
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly PnlService _pnlService;

        private readonly string EQUITY_TOPIC = "";

        public EquityPriceConsumer(PnlService pnlService)
        {
            _pnlService = pnlService;

            var config = new ConsumerConfig
            {
                GroupId = "equity-price-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public async Task ConsumeEquityPricesAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(EQUITY_TOPIC);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    HandlePriceUpdate(_consumer.Consume());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _consumer.Close();
            }
        }

        private void HandlePriceUpdate(ConsumeResult<Ignore, string> message)
        {
            Console.WriteLine($"Recieved: {message.Message.Value}");

            var priceUpdate = JsonSerializer.Deserialize<PriceUpdate>(message.Message.Value);

            if (priceUpdate != null)
            {
                _pnlService.HandlePriceUpdate(priceUpdate.Ticker, priceUpdate.Price);
            }
        }
    } 
}
