using MediatR;
using Shared.DataTransferObjects;

namespace Application.Commands;

public sealed record CreateTransactionCommand(TransactionForCreationDto TransactionForCreationDto) : IRequest<TransactionDto>;