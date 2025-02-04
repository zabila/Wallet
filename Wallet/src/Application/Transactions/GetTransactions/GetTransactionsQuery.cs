using Application.Messaging;
using SharedKernel.DTO.Transactions;

namespace Application.Transactions.GetTransactions;

public sealed record GetTransactionsQuery(Guid UserId) : IQuery<List<TransactionsResponse>>;
