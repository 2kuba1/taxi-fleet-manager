using Application.Contracts.Persistence;
using Application.Models.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

    public async Task<List<UserReportsDto>> GetTeamReportsInPeriodAsync(DateTime sinceWhen, DateTime untilWhen, Guid teamId)
    {
        var groupedData = await dbContext.ShiftReports
            .AsNoTracking()
            .Where(x => x.User.TeamId == teamId 
                        && x.ShiftDate >= sinceWhen 
                        && x.ShiftDate <= untilWhen)
            .Select(x => new
            {
                x.UserId,
                x.User.FirstName,
                x.User.LastName,
                Report = new TeamReportDto(
                    x.ReportStatus,
                    x.KilometersDriven.Value,
                    x.CardTransactionsSum.Value,
                    x.ShiftDate,
                    x.CarId,
                    x.Car != null ? x.Car.Brand : null,
                    x.Car != null ? x.Car.Model : null,
                    x.Car != null ? x.Car.LicensePlate.Value : null
                )
            })
            .GroupBy(x => new { x.UserId, x.FirstName, x.LastName })
            .ToListAsync();

        var userReports = groupedData.Select(g => new UserReportsDto(
            g.Key.UserId,
            g.Key.FirstName,
            g.Key.LastName,
            g.Select(r => r.Report)
                .OrderByDescending(r => r.ShiftDate)
                .ToList()
        )).ToList();
        
        return userReports;
    }
}