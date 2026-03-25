namespace PnlEngine.Interfaces
{
    public interface ITickerUserIndex
    {
        Task<HashSet<string>> GetUsersFromTicker(string ticker);
        Task AddUserToTicker(string ticker, string user);
    }
}
