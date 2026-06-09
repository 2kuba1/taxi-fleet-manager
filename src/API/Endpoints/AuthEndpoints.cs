using Application.Features.Auth.Commands;
using Cortex.Mediator;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth");

        group.MapPost("/register", Register);
    }

    private static async Task<IResult> Register([FromBody] RegisterBody body, [FromServices] IMediator mediator)
    {
        await mediator.SendCommandAsync(new RegisterCommand(body.Email, body.FirstName, body.LastName, body.PhoneNumber, body.AreaCode, body.KilometerRate, body.ContractType, body.TeamId));
        return Results.NoContent();
    }

    private record RegisterBody(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string AreaCode,
        float KilometerRate,
        ContractType ContractType,
        Guid TeamId);
}