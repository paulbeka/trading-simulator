using MarketDataService.KafkaProducers;
using MarketDataService.Models;
using MarketDataService.Providers;
using System.Text.Json;

namespace MarketDataService.Workers
{
    internal class MarketDataWorker : BackgroundService
    {
        private readonly int DELAY = 1000;

        private readonly YahooFinanceProvider _provider;
        private readonly KafkaProducer _producer;

        public MarketDataWorker()
        {
            _provider = new YahooFinanceProvider();
            _producer = new KafkaProducer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: fetch the tickers from the database
            var tickers = new[] { "AAPL", "GOOGL" };
            while (!stoppingToken.IsCancellationRequested)
            {
                var prices = await _provider.GetPricesAsync(tickers);
                Console.WriteLine(prices);
                foreach (var (ticker, price) in prices)
                {
                    var message = new MarketDataMessage
                    {
                        Ticker = ticker,
                        Price = price
                    };

                    var json = JsonSerializer.Serialize(message);

                    await _producer.PublishAsync("marketdata.equities", json);

                    Console.WriteLine($"Published: {ticker} {price}");
                }

                await Task.Delay(DELAY, stoppingToken);
            }
        }
    }
}
