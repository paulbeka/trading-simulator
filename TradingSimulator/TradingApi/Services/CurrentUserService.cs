using Database.DbContext;
using Microsoft.EntityFrameworkCore;
using TradingApi.Contracts.Users;
using TradingApi.Services.Interfaces;

namespace TradingApi.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly TradingDbContext _dbContext;

    public CurrentUserService(TradingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MeResponse> GetMeAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return new MeResponse
        {
            Id = user.Id,
            Email = user.Email,
        };
    }
}