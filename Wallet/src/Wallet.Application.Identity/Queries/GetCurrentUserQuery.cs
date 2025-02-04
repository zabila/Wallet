using MediatR;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Identity.Queries;

public sealed record GetCurrentUserQuery(string Username) : IRequest<CurrentUserDto>
{
}
