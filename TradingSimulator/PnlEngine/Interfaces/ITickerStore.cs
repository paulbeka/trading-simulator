namespace PnlEngine.Interfaces
{
    internal interface ITickerStore
    {
        public Task AddTicker(string ticker);
    }
}
