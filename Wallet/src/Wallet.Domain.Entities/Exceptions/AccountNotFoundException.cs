namespace Wallet.Domain.Entities.Exceptions;

public class AccountNotFoundException : NotFoundException
{
    public AccountNotFoundException() : base("The account doesn't exist in the database.") { }

    public AccountNotFoundException(Guid accountId) : base($"The account with id: {accountId} doesn't exist in the database.") { }

    public AccountNotFoundException(string message) : base(message) { }

    public AccountNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
