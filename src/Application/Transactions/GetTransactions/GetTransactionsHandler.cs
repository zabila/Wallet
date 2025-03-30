using Application.Data;
using Application.Messaging;
using Domain.Transactions;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.DTO.Transactions;

namespace Application.Transactions.GetTransactions;

internal sealed class GetTransactionsHandler(IRepositoryManager repository) : IQueryHandler<GetTransactionsQuery, List<TransactionsResponse>>
{
    public async Task<Result<List<TransactionsResponse>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.Users.FindByCondition(user => user.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            return Result.Failure<List<TransactionsResponse>>(UserErrors.NotFound(request.UserId));
        }

        var transactions = await repository.Transactions
            .FindByCondition(transaction => transaction.UserId == request.UserId && transaction.AccountId == user.AccountId)
            .Select(transaction => new TransactionsResponse
                {
                    Id = transaction.Id,
                    Date = transaction.UpdatedAt,
                    Amount = transaction.Amount,
                    Category = transaction.Category,
                    Type = transaction.Type
                }
            ).ToListAsync(cancellationToken);

        if (transactions == null || !transactions.Any())
        {
            return Result.Failure<List<TransactionsResponse>>(TransactionsErrors.NotFound(request.UserId));
        }

        return transactions;
    }
}
