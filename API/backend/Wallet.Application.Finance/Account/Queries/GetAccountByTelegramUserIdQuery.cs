using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Finance.Account.Queries;

public sealed record GetAccountByTelegramUserIdQuery(int TelegramUserId) : IRequest<AccountReadDto>;