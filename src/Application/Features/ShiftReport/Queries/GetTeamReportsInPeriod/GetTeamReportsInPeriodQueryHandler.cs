using Application.Contracts.Persistence;
using Application.Models.Responses;
using Cortex.Mediator.Queries;

namespace Application.Features.ShiftReport.Queries.GetTeamReportsInPeriod;

public sealed class GetTeamReportsInPeriodQueryHandler(IShiftReportService reportService) : IQueryHandler<GetTeamReportsInPeriodQuery, TeamReportResponse>
{
    public async Task<TeamReportResponse> Handle(GetTeamReportsInPeriodQuery query, CancellationToken cancellationToken)
    {
        var results = await reportService.GetTeamReportsInPeriodAsync(query.SinceWhen, query.UntilWhen, query.TeamId);
        return new TeamReportResponse(query.TeamId, results);
    }
}