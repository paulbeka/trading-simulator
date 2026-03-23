using Microsoft.AspNetCore.SignalR;

namespace TradingApi.Hubs
{
    public class PnlHub : Hub
    {
        public async Task Subscribe(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task Unsubscribe(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
    }
}
