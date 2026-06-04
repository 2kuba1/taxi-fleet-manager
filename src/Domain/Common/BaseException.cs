using System.Net;

namespace Domain.Common;

public abstract class BaseException(string message) : Exception(message)
{
    public abstract HttpStatusCode StatusCode { get; }
}