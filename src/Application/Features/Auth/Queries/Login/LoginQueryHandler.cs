using Application.Contracts.Persistence;
using Application.Models.DTOs;
using Cortex.Mediator.Queries;
using Domain.Exceptions;

namespace Application.Features.Auth.Queries.Login;

public sealed class LoginQueryHandler(
    IIdentityService identityService,
    IUserService userService,
    ITokenService tokenService) : IQueryHandler<LoginQuery, TokenResponseDto>
{
    public async Task<TokenResponseDto> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var validCredentials = await identityService.CheckLoginCredentialsAsync(query.Login, query.Password);
        
        if(!validCredentials)
            throw new InvalidCredentialsException("Invalid credentials");

        var domainUser = await userService.GetUserByLoginAsync(query.Login);

        if (domainUser == null)
            throw new InvalidCredentialsException("Invalid credentials");
        
        var accessToken = tokenService.CreateAccessToken(domainUser);
        var refreshToken = await tokenService.CreateRefreshToken(domainUser.Id);
        
        return new TokenResponseDto(accessToken, refreshToken.rawRefreshToken);
    }
}