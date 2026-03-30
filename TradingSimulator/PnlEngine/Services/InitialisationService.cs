using Database.DbContext;
using Microsoft.EntityFrameworkCore;
using PnlEngine.Interfaces;
using PnlEngine.Models;

namespace PnlEngine.Services
{
    internal class InitialisationService
    {
        private readonly TradingDbContext _db;
        private readonly IUserPositionStore _userPositionStore;
        private readonly ITickerUserIndex _tickerUserIndex;
        private readonly ITickerStore _tickerStore;

        public InitialisationService(
            TradingDbContext db,
            IUserPositionStore userPositionStore,
            ITickerUserIndex tickerUserIndex,
            ITickerStore tickerStore)
        {
            _db = db;
            _userPositionStore = userPositionStore;
            _tickerUserIndex = tickerUserIndex;
            _tickerStore = tickerStore;
        }

        public async Task LoadPositionsIntoRedis()
        {
            await foreach (var dbPosition in _db.Positions
                .AsNoTracking()
                .AsAsyncEnumerable())
            {
                var userId = dbPosition.UserId.ToString();
                var ticker = dbPosition.Symbol;

                var position = new Position
                {
                    Quantity = dbPosition.Quantity,
                    AvgEntryPrice = dbPosition.AvgEntryPrice
                };

                await _userPositionStore.Set(userId, ticker, position);
                await _tickerUserIndex.AddUserToTicker(ticker, userId);
                await _tickerStore.AddTicker(ticker);
            }
        }
    }
}