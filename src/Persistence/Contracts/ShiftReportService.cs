using Application.Contracts.Persistence;
using Domain.Entities;
using Persistence.Database;

namespace Persistence.Contracts;

public class ShiftReportService(AppDbContext dbContext) : IShiftReportService
{
    public async Task CreateShiftReportAsync(string imageUrl, int kilometersDriven, float cardTransactionsSum, Guid userId, DateTime shiftDay, Guid? carId = null)
    {
        var shiftReport = ShiftReport.Create(userId, imageUrl, kilometersDriven, cardTransactionsSum, shiftDay, carId);
        await dbContext.ShiftReports.AddAsync(shiftReport);
        await dbContext.SaveChangesAsync();
    }
}