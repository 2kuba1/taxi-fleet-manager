using Application.Models.Responses;
using Cortex.Mediator.Commands;

namespace Application.Features.Auth.Commands.RefreshAuthToken;

public record RefreshAuthTokenCommand(string RefreshToken) : ICommand<TokenResponse>;