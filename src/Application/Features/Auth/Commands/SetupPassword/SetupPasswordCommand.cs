using Cortex.Mediator.Commands;

namespace Application.Features.Auth.Commands.SetupPassword;

public record SetupPasswordCommand(string TemporaryPassword, string Token, string Email, string NewPassword) : ICommand;