using Database.DbContext;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using TradingApi.Contracts.Auth;
using TradingApi.Services.Interfaces;

namespace TradingApi.Services;

public sealed class AuthService : IAuthService
{
    private readonly TradingDbContext _dbContext;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        TradingDbContext dbContext,
        IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var emailExists = await _dbContext.Users
            .AnyAsync(x => x.Email == email, cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException("Email is already in use.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var (token, expiresAtUtc) = _jwtTokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        };
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var (token, expiresAtUtc) = _jwtTokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            ExpiresAtUtc = expiresAtUtc
        };
    }
}