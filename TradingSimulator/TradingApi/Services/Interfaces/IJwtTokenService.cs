using Database.Entities;

namespace TradingApi.Services.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAtUtc) GenerateToken(User user);
}