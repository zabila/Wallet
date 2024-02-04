using MediatR;
using Shared.DataTransferObjects;

namespace Wallet.App.Transaction.Queries;

public sealed record GetTransactionQuery(Guid AccountId, Guid Id, bool TrackChanges) : IRequest<TransactionReadDto>;