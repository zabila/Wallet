using MediatR;
using Shared.DataTransferObjects;

namespace Application.Transaction.Queries;

public sealed record GetTransactionsQuery(Guid AccountId, bool TrackChanges) : IRequest<IEnumerable<TransactionReadDto>>;