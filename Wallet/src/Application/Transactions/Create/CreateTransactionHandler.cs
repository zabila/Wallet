using Application.Data;
using Application.Messaging;
using Domain.Accounts;
using Domain.Transactions;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Transactions.Create;

internal sealed class CreateTransactionHandler(IRepositoryManager repositoryManager) : ICommandHandler<CreateTransactionCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await repositoryManager.Users.FindByCondition(u => u.Id == request.UserId).SingleOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(request.UserId));
        }

        var isAccountExists = await repositoryManager.Accounts.FindByCondition(account => account.Id == user.AccountId).AnyAsync(cancellationToken);
        if (!isAccountExists)
        {
            return Result.Failure<Guid>(AccountErrors.NotFound(user.AccountId));
        }

        var transaction = new Transaction
        {
            Amount = request.Amount,
            Category = request.Category,
            Type = request.Type,
            Location = request.Location,
            Attachment = request.Attachment,
            AccountId = user.AccountId,
            UserId = request.UserId
        };

        await repositoryManager.Transactions.CreateAsync(transaction, cancellationToken);
        await repositoryManager.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}
