using FluentValidation;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");
        
        RuleFor(x => x.AreaCode)
            .NotEmpty().WithMessage("Area Code is required")
            .MaximumLength(4).WithMessage("Area Code must not exceed 4 caracters")
            .MinimumLength(1).WithMessage("Area Code must contain at least 1 character");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is required")
            .MaximumLength(9).WithMessage("Phone Number must not exceed 9 characters");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.KilometerRate)
            .NotEmpty().WithMessage("Kilometer rate is required")
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ContractType)
            .NotEmpty().WithMessage("Contract type is required");
    }
}