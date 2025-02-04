namespace Wallet.Domain.Entities.Exceptions;

public abstract class BadRequestException : Exception
{
    protected BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BadRequestException(string? message) : base(message)
    {
    }

    protected BadRequestException() : base()
    {
    }
}
