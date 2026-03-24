using Microsoft.AspNetCore.SignalR;
using TradingApi.Contracts.MarketData;
using TradingApi.Hubs;
using TradingApi.Services.Interfaces;

namespace TradingApi.Services
{
    public class MarketDataService : IMarketDataService
    {
        private readonly IHubContext<PnlHub> _hubContext;

        public MarketDataService(IHubContext<PnlHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastTickerUpdate(MarketDataMessage message)
        {
            await _hubContext.Clients
                .Group(message.Ticker)
                .SendAsync("ReceiveTickerUpdate", message);
        }
    }
}
