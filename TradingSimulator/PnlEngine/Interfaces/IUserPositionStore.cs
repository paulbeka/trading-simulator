using PnlEngine.Models;

namespace PnlEngine.Interfaces
{
    public interface IUserPositionStore
    {
        Task<Position?> Get(string user, string ticker);
        Task Set(string user, string ticker, Position position);
    }
}
