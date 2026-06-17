using Cortex.Mediator.Commands;

namespace Application.Features.ShiftReport.Commands.CreateShiftReport;

public record CreateShiftReportCommand(Stream Image, int KilometersDriven, float CardTransactionsSum, string FileName, DateTime ShiftDay, Guid? CarId = null) : ICommand;