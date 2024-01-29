using Application.Transaction.Commands;
using AutoMapper;
using Contracts;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Transaction.Handlers;

internal sealed class CreateTransactionHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger) : IRequestHandler<CreateTransactionCommand, TransactionReadDto>
{
    public async Task<TransactionReadDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        logger.LogDebug($"CreateTransactionHandler: Creating transaction");

        var transactionDto = request?.TransactionForCreationDto;

        var accountId = request!.AccountId;
        var isAccountExists = repository.Account.AccountExists(accountId);
        if (!isAccountExists)
        {
            throw new ArgumentException($"Account with id {accountId} does not exist");
        }

        var transaction = mapper.Map<Entities.Model.Transaction>(transactionDto);
        transaction.AccountId = accountId;

        repository.Transaction.CreateTransaction(transaction);
        await repository.SaveAsync(cancellationToken);

        var transactionToReturn = mapper.Map<TransactionReadDto>(transaction);
        return transactionToReturn;
    }
}