using MediatR;
using Shared.DataTransferObjects;

namespace Application.Account.Queries;

public sealed record GetAccountByTelegramUserIdQuery(int TelegramUserId) : IRequest<AccountReadDto>;