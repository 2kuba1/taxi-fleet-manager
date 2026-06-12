using Application.Models.DTOs;
using Cortex.Mediator.Queries;

namespace Application.Features.Auth.Queries.Login;

public record LoginQuery(string Login, string Password) : IQuery<TokenResponseDto>;