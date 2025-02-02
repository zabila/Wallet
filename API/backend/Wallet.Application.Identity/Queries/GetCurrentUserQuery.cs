using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Identity.Queries;

public sealed record GetCurrentUserQuery(string Username) : IRequest<CurrentUserDto> {
}
