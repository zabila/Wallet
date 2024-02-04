using MediatR;
using Shared.DataTransferObjects;

namespace Application.Transaction.Queries;

public sealed record GetTransactionQuery(Guid AccountId, Guid Id, bool TrackChanges) : IRequest<TransactionReadDto>;