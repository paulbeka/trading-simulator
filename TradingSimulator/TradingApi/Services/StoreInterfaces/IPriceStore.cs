namespace TradingApi.Services.StoreInterfaces
{
    public interface IPriceStore
    {
        Task<decimal?> GetPrice(string symbol);
    }
}
