namespace Wallet.Domain.Entities.Exceptions;

public class AccountAlreadyExistsBadRequestException : BadRequestException
{
    public AccountAlreadyExistsBadRequestException(string name)
        : base($"Account with name {name} already exists.")
    {
    }

    public AccountAlreadyExistsBadRequestException()
        : base("Account already exists.")
    {
    }

    // Constructor with message parameter
    public AccountAlreadyExistsBadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
