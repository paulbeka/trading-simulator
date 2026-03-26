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

        public InitialisationService(
            TradingDbContext db,
            IUserPositionStore userPositionStore,
            ITickerUserIndex tickerUserIndex)
        {
            _db = db;
            _userPositionStore = userPositionStore;
            _tickerUserIndex = tickerUserIndex;
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
            }
        }
    }
}