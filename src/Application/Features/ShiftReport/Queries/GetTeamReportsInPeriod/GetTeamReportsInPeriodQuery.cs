using Application.Models.Responses;
using Cortex.Mediator.Queries;

namespace Application.Features.ShiftReport.Queries.GetTeamReportsInPeriod;

public record GetTeamReportsInPeriodQuery(DateTime SinceWhen, DateTime UntilWhen, Guid TeamId) : IQuery<TeamReportResponse>;