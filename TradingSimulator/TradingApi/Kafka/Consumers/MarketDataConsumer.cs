using TradingApi.Contracts.MarketData;
using TradingApi.Services.Interfaces;

namespace TradingApi.Kafka.Consumers
{
    public class MarketDataConsumer
    {
        private readonly IMarketDataService _marketDataService;

        public MarketDataConsumer(IMarketDataService marketDataService)
        {
            _marketDataService = marketDataService;
        }

        public async Task HandleMessage(MarketDataMessage message)
        {
            await _marketDataService.BroadcastTickerUpdate(message);
        }
    }
}
