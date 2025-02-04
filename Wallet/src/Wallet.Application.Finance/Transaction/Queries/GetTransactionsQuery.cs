using MediatR;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Finance.Transaction.Queries;

public sealed record GetTransactionsQuery(Guid AccountId, bool TrackChanges) : IRequest<IEnumerable<TransactionReadDto>>;