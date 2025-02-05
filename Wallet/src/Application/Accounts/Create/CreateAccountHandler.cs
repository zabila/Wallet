using Application.Data;
using Application.Messaging;
using Domain.Accounts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Accounts.Create;

internal sealed class CreateAccountHandler(IRepositoryManager repository) : ICommandHandler<CreateAccountCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.Users.FindByCondition(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(request.UserId));
        }

        if (user.AccountId != Guid.Empty)
        {
            return Result.Failure<Guid>(AccountErrors.AccountAlreadyExists(user.AccountId));
        }

        var account = new Account
        {
            AccountName = request.AccountName,
            AccountType = request.AccountType,
            Balance = request.Balance,
            Currency = request.Currency
        };

        await repository.Accounts.CreateAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}
