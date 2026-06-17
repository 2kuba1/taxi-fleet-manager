namespace Application.Contracts.Persistence;

public interface IShiftReportService
{
    Task CreateShiftReportAsync(string imageUrl, int kilometersDriven, float cardTransactionsSum, Guid userId, DateTime shiftDay, Guid? carId = null);
}