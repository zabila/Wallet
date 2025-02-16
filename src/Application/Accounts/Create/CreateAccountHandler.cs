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
        var user = await repository.Users.FindByCondition(u => u.Id == request.UserId, true).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound(request.UserId));
        }

        if (user.AccountId != null && user.AccountId != Guid.Empty)
        {
            return Result.Failure<Guid>(AccountErrors.AccountAlreadyExists(user.AccountId.Value));
        }

        var account = new Account
        {
            Id = Guid.NewGuid(),
            AccountName = request.AccountName,
            AccountType = request.AccountType,
            Balance = request.Balance,
            Currency = request.Currency
        };
        await repository.Accounts.CreateAsync(account, cancellationToken);

        user.Account = account;
        user.AccountId = account.Id;
        repository.Users.Update(user);

        await repository.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}
