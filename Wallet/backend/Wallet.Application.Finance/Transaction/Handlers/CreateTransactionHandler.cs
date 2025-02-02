using AutoMapper;
using MediatR;
using Wallet.Application.Finance.Transaction.Commands;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Exceptions;
using Wallet.Shared.DataTransferObjects;
using WalletTransaction = Wallet.Domain.Entities.Model.Transaction;

namespace Wallet.Application.Finance.Transaction.Handlers;

internal sealed class CreateTransactionHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger) : IRequestHandler<CreateTransactionCommand, TransactionReadDto> {
    public async Task<TransactionReadDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken) {
        logger.LogDebug($"CreateTransactionHandler: Creating transaction");

        var transactionDto = request?.TransactionForCreationDto;

        var accountId = request!.AccountId;
        var isAccountExists = repository.Account.AccountExists(accountId);
        if (!isAccountExists) {
            throw new AccountNotFoundException(accountId);
        }

        var transaction = mapper.Map<WalletTransaction>(transactionDto);
        transaction.AccountId = accountId;

        repository.Transaction.CreateTransaction(transaction);
        await repository.SaveAsync(cancellationToken);

        return mapper.Map<TransactionReadDto>(transaction);
    }
}