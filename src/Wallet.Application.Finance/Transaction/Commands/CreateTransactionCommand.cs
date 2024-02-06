using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Finance.Transaction.Commands;

public sealed record CreateTransactionCommand(Guid AccountId, TransactionCreateDto TransactionForCreationDto) : IRequest<TransactionReadDto>;