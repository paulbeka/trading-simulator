using TradingApi.Contracts.MarketData;

namespace TradingApi.Services.Interfaces
{
    public interface IMarketDataService
    {
        Task BroadcastTickerUpdate(MarketDataMessage message);
    }
}
