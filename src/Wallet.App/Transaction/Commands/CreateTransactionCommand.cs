﻿using MediatR;
using Shared.DataTransferObjects;

namespace Wallet.App.Transaction.Commands;

public sealed record CreateTransactionCommand(Guid AccountId, TransactionCreateDto TransactionForCreationDto) : IRequest<TransactionReadDto>;