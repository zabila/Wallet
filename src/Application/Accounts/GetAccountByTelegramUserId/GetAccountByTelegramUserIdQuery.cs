using Application.Messaging;
using SharedKernel.DTO.Accounts;

namespace Application.Accounts.GetAccountByTelegramUserId;

public sealed record GetAccountByTelegramUserIdQuery(Guid userid, long TelegramUserId) : IQuery<AccountResponse>;
