using Application.Data;
using Application.Messaging;
using Domain.Accounts;
using Domain.Transactions;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.DTO.Accounts;

namespace Application.Accounts.GetAccountByTelegramUserId;

internal sealed class GetAccountByTelegramUserIdHandler(IRepositoryManager repository) : IQueryHandler<GetAccountByTelegramUserIdQuery, AccountResponse>
{
    public async Task<Result<AccountResponse>> Handle(GetAccountByTelegramUserIdQuery request, CancellationToken cancellationToken)
    {
        var telegramUser = await repository.TelegramUsers.FindByCondition(userTelegram => userTelegram.TelegramUserId == request.TelegramUserId).FirstOrDefaultAsync(cancellationToken);
        if (telegramUser is null)
        {
            return Result.Failure<AccountResponse>(UserErrors.TelegreamUserNotFound(request.TelegramUserId));
        }

        var user = await repository.Users.FindByCondition(user => user.Id == telegramUser.UserId && user.Id == request.userid).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return Result.Failure<AccountResponse>(UserErrors.NotFound(telegramUser.UserId));
        }

        var account = await repository.Accounts.FindByCondition(a => a.Id == user.AccountId).FirstOrDefaultAsync(cancellationToken);
        if (account is null)
        {
            return Result.Failure<AccountResponse>(AccountErrors.NotFound(user.AccountId));
        }

        var accountResponse = new AccountResponse
        {
            Id = account.Id,
            AccountName = account.AccountName,
            AccountType = account.AccountType,
            Balance = account.Balance,
            Currency = account.Currency
        };

        return accountResponse;
    }
}
