using MediatR;
using Shared.DataTransferObjects;

namespace Application.Commands;

public sealed record CreateTransactionCommand(TransactionCreateDto TransactionForCreationDto) : IRequest<TransactionReadDto>;