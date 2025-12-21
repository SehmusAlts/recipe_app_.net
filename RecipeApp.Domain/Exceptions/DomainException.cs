namespace RecipeApp.Domain.Exceptions;

/// <summary>
/// Base exception for domain-level errors.
/// Follows Open/Closed Principle by being extensible.
/// </summary>
public class DomainException : Exception
{
    public DomainException() : base()
    {
    }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
