using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Persistence.Database;

namespace Persistence.Contracts;

public class TokenService(IConfiguration configuration, AppDbContext dbContext) : ITokenService
{
    public async Task<(RefreshToken refreshToken, string rawRefreshToken)> CreateRefreshToken(Guid userId)
    {
        var rawToken = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(64));
        
        var hashedToken = HashToken(rawToken);
        
        var expiresAt = DateTime.UtcNow.AddHours(configuration.GetValue<int>("Jwt:RefreshTokenExpirationInDays"));

        var refreshToken = RefreshToken.Create(hashedToken, userId, expiresAt);
        
        await dbContext.RefreshTokens.AddAsync(refreshToken);
        await dbContext.SaveChangesAsync();
        
        return (refreshToken, rawToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        var tokenHash = HashToken(refreshToken);
        return await dbContext.RefreshTokens.Include(u => u.User)
            .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);
    }

    public async Task RevokeOldRefreshTokenAsync(RefreshToken refreshToken)
    {
        dbContext.RefreshTokens.Remove(refreshToken);
        await dbContext.SaveChangesAsync();
    }

    public string CreateAccessToken(User user)
    {
        var secretKey = configuration["Jwt:Secret"];
        if(secretKey is null)
            throw new InvalidOperationException("Missing configuration secret");
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:AccessTokenExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
        };
        
        var claimsIdentity = new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "Driver"),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
        ]);
        
        tokenDescriptor.Subject = claimsIdentity;

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);
        
        return token;
    }
    
    private string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}