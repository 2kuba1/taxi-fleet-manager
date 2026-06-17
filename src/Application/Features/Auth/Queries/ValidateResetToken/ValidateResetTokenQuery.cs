using Cortex.Mediator.Commands;
using Cortex.Mediator.Queries;

namespace Application.Features.Auth.Queries.ValidateResetToken;

public record ValidateResetTokenQuery(string Token, string Email) : IQuery<string>;