using Application.Contracts.Persistence;
using Cortex.Mediator.Commands;

namespace Application.Features.Auth.Commands.SetupPassword;

public class SetupPasswordCommandHandler(IIdentityService identityService) : ICommandHandler<SetupPasswordCommand>
{
    public async Task Handle(SetupPasswordCommand command, CancellationToken cancellationToken)
    {
        await identityService.SetupPasswordAsync(command.Token, command.Email, command.TemporaryPassword, command.NewPassword);
    }
}