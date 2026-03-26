using Database.Entities;

namespace TradingApi.Services.StoreInterfaces
{
    public interface IUserPositionStore
    {
        Task<Position?> Get(string user, string ticker);
        Task Set(string user, string ticker, Position position);
        Task Remove(string user, string ticker);
    }
}
