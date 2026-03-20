
using PnlEngine.Stores;

namespace PnlEngine.Services
{
    internal class PnlService
    {
        private readonly PriceCache _priceCache;
        private readonly TickerToUserIndex _tickerToUserIndex;

        public PnlService(
            PriceCache priceCache,
            TickerToUserIndex tickerToUserIndex) 
        { 
            _priceCache = priceCache;
            _tickerToUserIndex = tickerToUserIndex;
        }

        public void HandlePriceUpdate(string ticker, decimal price) 
        {
            _priceCache.Update(ticker, price);
            HashSet<string> updateUsers = _tickerToUserIndex.GetUsersFromTicker(ticker);

            // TODO: if scaled, ensure only 1 instance handles 1 user
            foreach (string user in updateUsers)
            {

            }
        }

    }
}
