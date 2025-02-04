using MediatR;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Finance.Transaction.Commands;

public sealed record CreateTransactionCommand(Guid AccountId, TransactionCreateDto TransactionForCreationDto) : IRequest<TransactionReadDto>;