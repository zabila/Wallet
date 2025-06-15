using SharedKernel;

namespace Domain.Accounts;

public static class AccountErrors
{
    public static Error NotFound(Guid accountId)
    {
        return Error.NotFound(
            "Account.NotFound",
            $"The Account with the Id = '{accountId}' was not found");
    }

    public static Error NotFoundByUserId(Guid userId)
    {
        return Error.NotFound(
            "Account.NotFoundByUserId",
            $"The Account with the User Id = '{userId}' was not found");
    }

    public static Error AccountAlreadyExists(Guid accountId)
    {
        return Error.Conflict(
            "Account.AlreadyExists",
            $"The Account with the Id = '{accountId}' already exists");
    }
}
