using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Identity.Queries;

public sealed record GetCurrentUserByTelegramIdQuery(int TelegramId) : IRequest<CurrentUserDto> {
}