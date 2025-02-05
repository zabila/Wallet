using SharedKernel;

namespace Domain.Accounts;

public static class AccountErrors
{
    public static Error NotFound(Guid accountId) => Error.NotFound(
        "Account.NotFound",
        $"The Account with the Id = '{accountId}' was not found");


    public static Error AccountAlreadyExists(Guid accountId) => Error.Conflict(
        "Account.AlreadyExists",
        $"The Account with the Id = '{accountId}' already exists");
}
