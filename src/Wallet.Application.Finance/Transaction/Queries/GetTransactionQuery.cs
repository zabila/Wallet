using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Finance.Transaction.Queries;

public sealed record GetTransactionQuery(Guid AccountId, Guid Id, bool TrackChanges) : IRequest<TransactionReadDto>;