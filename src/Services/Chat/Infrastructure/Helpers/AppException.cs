namespace Infrastructure.Helpers;

using System.Net;

public class AppException : Exception
{
    public AppException() : base() {}

    public AppException(string message) : base(message)
    {
        throw new HttpStatusException((int)HttpStatusCode.BadRequest, message);
    }

    public AppException(string message, Exception innerException) : base(message, innerException)
    {
    }
}