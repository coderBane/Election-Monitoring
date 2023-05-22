using System.Globalization;

namespace Election2023.Domain.CustomExceptions;

public class ElectionDomainException : Exception
{
    public ElectionDomainException() : base() {}

    public ElectionDomainException(string message) : base(message) {}

    public ElectionDomainException(string message, params object[] args) 
        : base(string.Format(CultureInfo.CurrentCulture, message, args)) {}

    public ElectionDomainException(string message, Exception innerException) 
        : base(message, innerException) {}
}