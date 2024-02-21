using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Finance.Account.Commands;

public sealed record CreateAccountCommand(AccountCreateDto AccountCreateDto) : IRequest<AccountReadDto>;