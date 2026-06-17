using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Cortex.Mediator.Commands;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Application.Features.ShiftReport.Commands.CreateShiftReport;

public class CreateShiftReportCommandHandler(IShiftReportService shiftReportService, 
    IUserContext userContext, 
    ICdnService cdnService, 
    IUnitOfWork unitOfWork, 
    IConfiguration configuration) : ICommandHandler<CreateShiftReportCommand>
{
    public async Task Handle(CreateShiftReportCommand command, CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;
        
        if(userId == null || userId == Guid.Empty)
            throw new UserNotFoundException($"User with id {userId} not found");

        
        var fullFileName = command.FileName+"-"+userId;
        var imageUrl = configuration["Cdn:Url"] + "/" + fullFileName;

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            await shiftReportService.CreateShiftReportAsync(imageUrl, command.KilometersDriven,
                command.CardTransactionsSum, (Guid)userId, command.ShiftDay, command.CarId);
            
            await cdnService.SaveImageAsync(command.Image, fullFileName);
            
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}