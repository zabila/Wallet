using MediatR;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Finance.Account.Queries;

public sealed record GetAccountByTelegramUserIdQuery(int TelegramUserId) : IRequest<AccountReadDto>;