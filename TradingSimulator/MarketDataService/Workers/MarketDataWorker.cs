using MarketDataService.KafkaProducers;
using MarketDataService.Models;
using MarketDataService.Providers;
using MarketDataService.Redis.Interfaces;
using System.Text.Json;

namespace MarketDataService.Workers
{
    internal class MarketDataWorker : BackgroundService
    {
        private readonly int DELAY = 1000 * 1;

        private readonly PolygonProvider _provider;
        private readonly KafkaProducer _producer;
        private readonly ITickerStore _tickerStore;

        public MarketDataWorker(ITickerStore tickerStore)
        {
            _provider = new PolygonProvider();
            _producer = new KafkaProducer();
            _tickerStore = tickerStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tickers = await _tickerStore.GetAllTickers();
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