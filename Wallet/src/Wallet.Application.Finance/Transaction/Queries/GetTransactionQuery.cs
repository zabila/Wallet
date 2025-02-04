using MediatR;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Finance.Transaction.Queries;

public sealed record GetTransactionQuery(Guid AccountId, Guid Id, bool TrackChanges) : IRequest<TransactionReadDto>;