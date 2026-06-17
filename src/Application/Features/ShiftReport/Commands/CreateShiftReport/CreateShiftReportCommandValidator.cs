using FluentValidation;

namespace Application.Features.ShiftReport.Commands.CreateShiftReport;

public class CreateShiftReportCommandValidator : AbstractValidator<CreateShiftReportCommand>
{
    public CreateShiftReportCommandValidator()
    {
        RuleFor(c => c.Image)
            .NotNull()
            .NotEmpty()
            .WithMessage("Please specify an image");
        
        RuleFor(c => c.FileName)
            .NotNull()
            .NotEmpty()
            .WithMessage("File does not have a name");
        
        RuleFor(c => c.CardTransactionsSum)
            .NotNull()
            .WithMessage("Please specify a total sum");
 
        RuleFor(c => c.KilometersDriven)
            .NotNull()
            .WithMessage("Please specify a total kilometers driven");
        
        RuleFor(c => c.ShiftDay)
            .NotNull()
            .WithMessage("Please specify a shift day");
    }
}