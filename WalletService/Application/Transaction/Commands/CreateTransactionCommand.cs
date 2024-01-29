using MediatR;
using Shared.DataTransferObjects;

namespace Application.Transaction.Commands;

public sealed record CreateTransactionCommand(TransactionCreateDto TransactionForCreationDto) : IRequest<TransactionReadDto>;