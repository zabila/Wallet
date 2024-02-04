using Application.Transaction.Queries;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Transaction.Handlers;

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