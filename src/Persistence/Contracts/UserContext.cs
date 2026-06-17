using System.Security.Claims;
using Application.Contracts.Persistence;
using Microsoft.AspNetCore.Http;

namespace Persistence.Contracts;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid? UserId { get; set; } = Guid.Parse(httpContextAccessor.HttpContext?.User.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
        ?.Value!);
}