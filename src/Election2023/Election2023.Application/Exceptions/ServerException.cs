using System.Net;

namespace Election2023.Application.Exceptions;

public class ServerException : Exception
{
    public ServerException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorMessages = new string[] { message };
        StatusCode = statusCode;
    }

    public IEnumerable<string> ErrorMessages { get; }
    public HttpStatusCode StatusCode { get; }
}