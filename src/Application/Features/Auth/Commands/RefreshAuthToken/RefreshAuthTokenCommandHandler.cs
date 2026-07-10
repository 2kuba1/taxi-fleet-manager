using Application.Contracts.Persistence;
using Application.Models.Responses;
using Cortex.Mediator.Commands;
using Domain.Exceptions;

namespace Application.Features.Auth.Commands.RefreshAuthToken;

public sealed class RefreshAuthTokenCommandHandler(ITokenService tokenService) : ICommandHandler<RefreshAuthTokenCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(RefreshAuthTokenCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = await tokenService.GetRefreshTokenAsync(command.RefreshToken);
        
        if(refreshToken == null || refreshToken.ExpiresAtUtc < DateTime.UtcNow)
            throw new RefreshTokenNotFound("Refresh token not found");

        var accessToken = tokenService.CreateAccessToken(refreshToken.User); 
        var newRefreshToken = await tokenService.CreateRefreshToken(refreshToken.UserId);

        await tokenService.RevokeOldRefreshTokenAsync(refreshToken);
        
        return new TokenResponse(accessToken, newRefreshToken.rawRefreshToken);
    }
}