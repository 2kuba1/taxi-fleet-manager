using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface ITokenService
{
    string CreateAccessToken(User user);
    Task<(RefreshToken refreshToken, string rawRefreshToken)> CreateRefreshToken(Guid userId);
    Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);
    Task RevokeOldRefreshTokenAsync(RefreshToken refreshToken);
}