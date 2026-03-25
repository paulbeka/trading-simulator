namespace PnlEngine.Interfaces
{
    public interface IPriceStore
    {
        Task<(bool changed, decimal? oldPrice)> UpdateIfChanged(string symbol, decimal newPrice);
        Task<decimal?> GetPrice(string symbol);
    }
}
