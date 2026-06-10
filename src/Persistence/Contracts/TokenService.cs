using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Persistence.Database;

namespace Persistence.Contracts;

public class TokenService(IConfiguration configuration, AppDbContext dbContext) : ITokenService
{
    public async Task<RefreshToken> CreateRefreshToken(Guid userId)
    {
        var token = GenerateHashedRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:RefreshTokenExpirationInDays"));

        var refreshToken = RefreshToken.Create(token, userId, expiresAt);
        
        await dbContext.RefreshTokens.AddAsync(refreshToken);
        await dbContext.SaveChangesAsync();
        
        return refreshToken;
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
    
    private string GenerateHashedRefreshToken()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}