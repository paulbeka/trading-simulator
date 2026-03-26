namespace TradingApi.Services.StoreInterfaces
{
    public interface ITickerUserIndex
    {
        Task<HashSet<string>> GetUsersFromTicker(string ticker);
        Task AddUserToTicker(string ticker, string user);
        Task RemoveUserFromTicker(string ticker, string user);
    }
}
