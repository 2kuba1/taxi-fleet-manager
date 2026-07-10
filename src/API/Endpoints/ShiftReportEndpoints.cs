using Application.Features.ShiftReport.Commands.CreateShiftReport;
using Application.Features.ShiftReport.Queries.GetTeamReportsInPeriod;
using Cortex.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

public static class ShiftReportEndpoints
{
    public static IEndpointRouteBuilder MapShiftReportEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/shift-report");

        group.MapPost("/create", CreateWorkShiftReport)
            .DisableAntiforgery()
            .RequireAuthorization();

        group.MapGet("/getTeamReportsInPeriod", GetTeamReportsInPeriod)
            .RequireAuthorization("ManagementOnly");
        
        return endpoints;
    }

    private static async Task<IResult> CreateWorkShiftReport([FromForm] CreateWorkShiftReportBody body, [FromServices] IMediator mediator)
    {
        await using var stream = body.Image.OpenReadStream();
        await mediator.SendCommandAsync(new CreateShiftReportCommand(stream, body.KilometersDriven, body.CardTransactionsSum, body.Image.FileName, body.ShiftDate, body.CarId));
        return Results.NoContent();
    }

    private static async Task<IResult> GetTeamReportsInPeriod([FromQuery] DateTime sinceWhen, [FromQuery] DateTime untilWhen, [FromQuery] Guid teamId, [FromServices] IMediator mediator)
    {
        var results = await mediator.SendQueryAsync(new GetTeamReportsInPeriodQuery(sinceWhen, untilWhen, teamId));
        return Results.Ok(results);
    }

    private record CreateWorkShiftReportBody
    {
        public IFormFile Image { get; init; }
        public int KilometersDriven { get; init; }
        public float CardTransactionsSum { get; init; }
        public DateTime ShiftDate { get; init; }
        public Guid? CarId { get; init; } = null;
    }
}