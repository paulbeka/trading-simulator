namespace MarketDataService.Redis.Interfaces
{
    internal interface ITickerStore
    {
        public Task<IEnumerable<string>> GetAllTickers();
    }
}
