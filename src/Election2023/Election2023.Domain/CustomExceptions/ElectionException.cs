namespace Election2023.Domain.CustomExceptions;

public class ElectionDomainException : Exception
{
    public ElectionDomainException() {}

    public ElectionDomainException(string message) : base(message) {}

    public ElectionDomainException(string message, Exception innerException) 
        : base(message, innerException) {}
}