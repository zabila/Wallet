using MediatR;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Finance.Account.Commands;

public sealed record CreateAccountCommand(AccountCreateDto AccountCreateDto) : IRequest<AccountReadDto>;