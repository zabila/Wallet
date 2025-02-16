using Application.Data;
using Application.Messaging;
using Domain.Accounts;
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

        var user = telegramUser.User;
        if (user is null)
        {
            return Result.Failure<AccountResponse>(UserErrors.NotFound(telegramUser.UserId));
        }

        var account = user.Account;
        if (account is null)
        {
            return Result.Failure<AccountResponse>(AccountErrors.NotFoundByUserId(user.Id));
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
