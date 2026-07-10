using Application.Features.ShiftReport.Queries.GetTeamReportsInPeriod;
using Application.Models.DTOs;

namespace Application.Contracts.Persistence;

public interface IShiftReportService
{
    Task CreateShiftReportAsync(string imageUrl, int kilometersDriven, float cardTransactionsSum, Guid userId, DateTime shiftDay, Guid? carId = null);
    Task<List<UserReportsDto>> GetTeamReportsInPeriodAsync(DateTime sinceWhen, DateTime untilWhen, Guid teamId);
}