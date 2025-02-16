using SharedKernel;

namespace Domain.Transactions;

public static class TransactionsErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Transaction.NotFound",
        $"The user with the Id = '{userId}' was not found");
}
