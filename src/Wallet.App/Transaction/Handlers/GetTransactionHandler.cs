using AutoMapper;
using MediatR;
using Wallet.Shared.DataTransferObjects;
using Wallet.App.Transaction.Queries;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Exceptions;

namespace Wallet.App.Transaction.Handlers;

internal sealed class GetTransactionHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IRequestHandler<GetTransactionQuery, TransactionReadDto>
{
    public async Task<TransactionReadDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug($"GetTransactionHandler: Getting transaction with id {request.Id}");

        var accountId = request!.AccountId;
        var isAccountExists = repository.Account.AccountExists(accountId);
        if (!isAccountExists)
        {
            throw new AccountNotFoundException(accountId);
        }

        var transactionId = request.Id;
        var transaction = await repository.Transaction.GetTransactionAsync(transactionId, request.TrackChanges, cancellationToken);
        return mapper.Map<TransactionReadDto>(transaction);
    }
}