using TradingApi.Contracts.Users;

namespace TradingApi.Services.Interfaces;

public interface ICurrentUserService
{
    Task<MeResponse> GetMeAsync(Guid userId, CancellationToken cancellationToken);
}