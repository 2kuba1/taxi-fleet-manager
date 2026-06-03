using System;
using System.Net;

namespace Domain.Common;

public abstract class BaseException : Exception
{
    public BaseException(string message) : base(message)
    {
        
    }

    public HttpStatusCode StatusCode { get; set; }
}