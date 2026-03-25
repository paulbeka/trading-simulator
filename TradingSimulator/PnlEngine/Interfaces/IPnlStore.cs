using System;

namespace PnlEngine.Interfaces
{
    public interface IPnlStore
    {
        Task ChangePnlWithDelta(string user, decimal delta);
        Task<decimal> Get(string user);
    }
}
