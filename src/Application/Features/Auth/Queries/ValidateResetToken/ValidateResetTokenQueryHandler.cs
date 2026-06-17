using Application.Contracts.Persistence;
using Cortex.Mediator.Queries;
using Domain.Exceptions;

namespace Application.Features.Auth.Queries.ValidateResetToken;

public class ValidateResetTokenQueryHandler(IIdentityService identityService) : IQueryHandler<ValidateResetTokenQuery, string>
{
    public async Task<string> Handle(ValidateResetTokenQuery query, CancellationToken cancellationToken)
    {
        var isValid = await identityService.CheckPasswordResetTokenAsync(query.Token, query.Email);
        if (!isValid)
            throw new InvalidResetTokenException("Invalid reset token.");

        return "OK";
    }
}