using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface ITokenService
{
    string CreateAccessToken(User user);
    Task<RefreshToken> CreateRefreshToken(Guid userId);
}