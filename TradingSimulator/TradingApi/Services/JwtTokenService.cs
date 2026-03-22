using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Database.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TradingApi.Options;
using TradingApi.Services.Interfaces;

namespace TradingApi.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtConfig _jwtConfig;

    public JwtTokenService(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public (string Token, DateTime ExpiresAtUtc) GenerateToken(User user)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenValidityMins);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expiresAtUtc);
    }
}