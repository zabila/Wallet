using MediatR;
using Shared.DataTransferObjects;

namespace Application.Queries.Transaction;

public sealed record GetTransactionQuery(Guid Id, bool TrackChanges) : IRequest<TransactionDto>;