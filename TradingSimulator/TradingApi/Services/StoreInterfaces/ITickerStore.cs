namespace TradingApi.Services.StoreInterfaces
{
    public interface ITickerStore
    {
        Task AddTicker(string ticker);
        Task RemoveTicker(string ticker);
        Task<HashSet<string>> GetAllTickers();
    }
}
