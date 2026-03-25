using MarketDataService.KafkaProducers;
using MarketDataService.Models;
using MarketDataService.Providers;
using System.Text.Json;

namespace MarketDataService.Workers
{
    internal class MarketDataWorker : BackgroundService
    {
        private readonly int DELAY = 1000 * 1;

        private readonly PolygonProvider _provider;
        private readonly KafkaProducer _producer;

        public MarketDataWorker()
        {
            _provider = new PolygonProvider();
            _producer = new KafkaProducer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tickers = new[] { "AAPL", "GOOGL" };
                var prices = await _provider.GetPricesAsync(tickers);

                foreach (var (ticker, price) in prices)
                {
                    var message = new MarketDataMessage
                    {
                        Ticker = ticker,
                        Price = price
                    };

                    var json = JsonSerializer.Serialize(message);

                    await _producer.PublishAsync("marketdata.equities", ticker, json);

                    Console.WriteLine($"Published: {ticker} {price}");
                }

                await Task.Delay(DELAY, stoppingToken);
            }
        }
    }
}