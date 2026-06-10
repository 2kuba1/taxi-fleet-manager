using Application.Models.DTOs;
using Cortex.Mediator.Commands;

namespace Application.Features.Auth.Commands.RefreshAuthToken;

public record RefreshAuthTokenCommand(string RefreshToken) : ICommand<TokenResponseDto>;