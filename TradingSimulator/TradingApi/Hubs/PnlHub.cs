using Microsoft.AspNetCore.SignalR;
using TradingApi.Services.StoreInterfaces;

namespace TradingApi.Hubs
{
    public class PnlHub : Hub
    {
        private readonly ITickerStore _tickerStore;

        public PnlHub(ITickerStore tickerStore)
        {
            _tickerStore = tickerStore;
        }

        public async Task Subscribe(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task Unsubscribe(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SubscribeTicker(string ticker)
        {
            await _tickerStore.AddTicker(ticker);
            await Groups.AddToGroupAsync(Context.ConnectionId, ticker);
        }

        public async Task UnsubscribeTicker(string ticker)
        {
            await _tickerStore.AddTicker(ticker);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ticker);
        }
    }
}