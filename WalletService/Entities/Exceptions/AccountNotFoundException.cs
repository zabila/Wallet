namespace Entities.Exceptions;

public class AccountNotFoundException(Guid accountId) : NotFoundException($"The account with id: {accountId} doesn't exist in the database.");