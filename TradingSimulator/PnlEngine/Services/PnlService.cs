
using PnlEngine.Models;
using PnlEngine.Producer;
using PnlEngine.Stores;

namespace PnlEngine.Services
{
    internal class PnlService
    {
        private readonly PriceCache _priceCache;
        private readonly TickerToUserIndex _tickerToUserIndex;
        private readonly UserPositions _userPositions;
        private readonly PnlStore _pnlStore;
        private readonly PnlUpdateKafkaProducer _producer;

        public PnlService(
            PriceCache priceCache,
            TickerToUserIndex tickerToUserIndex,
            UserPositions userPositions,
            PnlStore pnlStore,
            PnlUpdateKafkaProducer producer) 
        { 
            _priceCache = priceCache;
            _tickerToUserIndex = tickerToUserIndex;
            _userPositions = userPositions;
            _pnlStore = pnlStore;
            _producer = producer;
        }

        public void InitialisePnl(List<string> users)
        {

        }

        public async void HandlePriceUpdate(string ticker, decimal price) 
        {
            var (changed, oldPrice) = _priceCache.UpdateIfChanged(ticker, price);

            if (!changed || oldPrice == null)
                return;
            
            HashSet<string> updateUsers = _tickerToUserIndex.GetUsersFromTicker(ticker);

            foreach (string user in updateUsers)
            {
                var position = _userPositions.Get(user, ticker);
                if (position == null) continue;

                var delta = (price - position.LastPrice) * position.Quantity;

                _pnlStore.ChangePnlWithDelta(user, delta);

                var totalPnl = _pnlStore.Get(user);
                var positionPnl = (price - position.EntryPrice) * position.Quantity;

                await _producer.PublishAsync(new PnLUpdate
                {
                    User = user,
                    Ticker = ticker,
                    Price = price,
                    PositionPnL = positionPnl,
                    PositionDelta = delta,
                    TotalPnL = totalPnl,
                    TotalDelta = delta,
                });

                position.LastPrice = price;
            }
        }

    }
}
