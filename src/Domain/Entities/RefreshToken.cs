using Domain.Common;

namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string TokenHash { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }

    public User User { get; private set; }
    
    protected RefreshToken(){}

    private RefreshToken(string tokenHash, Guid userId, DateTime expiresAtUtc)
    {
        TokenHash = tokenHash;
        UserId = userId;
        ExpiresAtUtc = expiresAtUtc;
    }

    public static RefreshToken Create(string tokenHash, Guid userId, DateTime expiresAtUtc)
    {
        if (string.IsNullOrEmpty(tokenHash))
            throw new ArgumentException("Token hash cannot be null or empty");

        if (expiresAtUtc < DateTime.UtcNow)
            throw new ArgumentException("Expires at utc cannot be created to the past");
        
        return new RefreshToken(tokenHash, userId, expiresAtUtc);
    }
}