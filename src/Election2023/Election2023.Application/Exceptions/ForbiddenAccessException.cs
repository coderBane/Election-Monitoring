namespace Election2023.Application.Exceptions;

public class ForbiddenAccessException : ServerException
{
    public ForbiddenAccessException(string message) : base(message, System.Net.HttpStatusCode.Forbidden)
    {

    }
}