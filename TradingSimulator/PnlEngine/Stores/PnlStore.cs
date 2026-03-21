using System.Collections.Concurrent;

namespace PnlEngine.Stores
{
    internal class PnlStore
    {
        private readonly ConcurrentDictionary<String, decimal> _pnl = new();

        public void Update(string key, decimal value)
        {
            _pnl[key] = value;
        }

        public decimal Get(string key)
        {
            return _pnl[key];
        }

        public void ChangePnlWithDelta(string key, decimal delta)
        {
            _pnl.AddOrUpdate(
                key,
                delta,
                (_, current) => current + delta 
            );
        }
    }
}
