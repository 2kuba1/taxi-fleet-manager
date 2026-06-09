using System.Net;
using Domain.Common;

namespace Domain.Exceptions;

public class RoleNotFoundException(string message) : BaseException(message)
{
    public override HttpStatusCode StatusCode { get; } =  HttpStatusCode.NotFound;
}