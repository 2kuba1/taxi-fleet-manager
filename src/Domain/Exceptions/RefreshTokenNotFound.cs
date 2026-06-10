using System.Net;
using Domain.Common;

namespace Domain.Exceptions;

public class RefreshTokenNotFound(string message) : BaseException(message)
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;
}