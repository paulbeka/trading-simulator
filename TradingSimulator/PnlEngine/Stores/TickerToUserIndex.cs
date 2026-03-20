using System.Collections.Concurrent;

namespace PnlEngine.Stores
{
    internal class TickerToUserIndex
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _tickerToUserDictionary = new();

        public void AddUser(string ticker, string user)
        {
            if (_tickerToUserDictionary.ContainsKey(ticker))
            {
                _tickerToUserDictionary[ticker].Add(user);
            }
            else
            {
                _tickerToUserDictionary[ticker] = [user];
            }
        }

        public void RemoveUser(string ticker, string user)
        {
            if (_tickerToUserDictionary.ContainsKey(ticker)) {
                _tickerToUserDictionary[ticker].Remove(user);
            }
        }

        public HashSet<string> GetUsersFromTicker(string ticker)
        {
            return _tickerToUserDictionary.GetValueOrDefault(ticker, []);
        }
    }
}
