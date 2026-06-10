using Cortex.Mediator.Commands;
using Domain.Enums;

namespace Application.Features.Auth.Commands;

public record RegisterCommand(string Email, string FirstName, string LastName, string PhoneNumber, string AreaCode, float KilometerRate, ContractType ContractType, Guid? TeamId) : ICommand;