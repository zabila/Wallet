using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Finance.Transaction.Queries;

public sealed record GetTransactionsQuery(Guid AccountId, bool TrackChanges) : IRequest<IEnumerable<TransactionReadDto>>;