using MediatR;
using Shared.DataTransferObjects;

namespace Application.Transaction.Commands;

public sealed record CreateTransactionCommand(Guid AccountId, TransactionCreateDto TransactionForCreationDto) : IRequest<TransactionReadDto>;