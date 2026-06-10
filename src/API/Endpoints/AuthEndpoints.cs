using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Queries.Login;
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
        group.MapPost("/login", Login);
    }

    private static async Task<IResult> Register([FromBody] RegisterBody body, [FromServices] IMediator mediator)
    {
        await mediator.SendCommandAsync(new RegisterCommand(body.Email, body.FirstName, body.LastName, body.PhoneNumber, body.AreaCode, body.KilometerRate, body.ContractType, body.TeamId));
        return Results.NoContent();
    }

    private static async Task<IResult> Login([FromBody] LoginBody body, [FromServices] IMediator mediator)
    {
        var tokens = await mediator.SendQueryAsync(new LoginQuery(body.Login, body.Password));
        return Results.Ok(tokens);
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

    private record LoginBody(
        string Login,
        string Password);
}