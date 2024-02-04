using MediatR;
using Shared.DataTransferObjects;

namespace Application.Account.Commands;

public sealed record CreateAccountCommand(AccountCreateDto AccountCreateDto) : IRequest<AccountReadDto>;