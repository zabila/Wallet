﻿using MediatR;
using Shared.DataTransferObjects;

namespace Wallet.App.Transaction.Queries;

public sealed record GetTransactionsQuery(Guid AccountId, bool TrackChanges) : IRequest<IEnumerable<TransactionReadDto>>;