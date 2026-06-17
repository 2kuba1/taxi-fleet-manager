using Application.Features.ShiftReport.Commands.CreateShiftReport;
using Cortex.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

public static class ShiftReportEndpoints
{
    public static IEndpointRouteBuilder MapShiftReportEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/workshift");

        group.MapPost("/create", CreateWorkShiftReport)
            .DisableAntiforgery()
            .RequireAuthorization();
        
        return endpoints;
    }

    private static async Task<IResult> CreateWorkShiftReport([FromForm] CreateWorkShiftReportBody body, [FromServices] IMediator mediator)
    {
        await using var stream = body.Image.OpenReadStream();
        await mediator.SendCommandAsync(new CreateShiftReportCommand(stream, body.KilometersDriven, body.CardTransactionsSum, body.Image.FileName, body.ShiftDay ,body.CarId));
        return Results.NoContent();
    }

    private record CreateWorkShiftReportBody
    {
        public IFormFile Image { get; init; }
        public int KilometersDriven { get; init; }
        public float CardTransactionsSum { get; init; }
        public DateTime ShiftDay { get; init; }
        public Guid? CarId { get; init; } = null;
    }
}