using Application.Features.Auth.Commands.RefreshAuthToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.SetupPassword;
using Application.Features.Auth.Queries.Login;
using Application.Features.Auth.Queries.ValidateResetToken;
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
        group.MapPost("/refresh", RefreshAuthToken);
        group.MapPost("/validate-reset-token", ValidateResetToken);
        group.MapPost("/setup-password", SetupPassword);
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

    private static async Task<IResult> RefreshAuthToken([FromBody] RefreshAuthTokenBody body,
        [FromServices] IMediator mediator)
    {
        var tokens = await mediator.SendCommandAsync(new RefreshAuthTokenCommand(body.RefreshToken));
        return Results.Ok(tokens);
    }

    private static async Task<IResult> ValidateResetToken([FromBody] ValidateResetTokenBody body, [FromServices] IMediator mediator)
    {
        var result = await mediator.SendQueryAsync(new ValidateResetTokenQuery(body.Token, body.Email));
        return Results.Ok(result);
    }

    private static async Task<IResult> SetupPassword([FromBody] SetupPasswordBody body,
        [FromServices] IMediator mediator)
    {
        await mediator.SendCommandAsync(new SetupPasswordCommand(body.TemporaryPassword, body.Token, body.Email, body.NewPassword));
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
        Guid? TeamId);

    private record LoginBody(
        string Login,
        string Password);

    private record RefreshAuthTokenBody(
        string RefreshToken
    );

    private record ValidateResetTokenBody(
        string Token,
        string Email);

    private record SetupPasswordBody(
        string TemporaryPassword,
        string Token,
        string Email,
        string NewPassword);
}