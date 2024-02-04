using AutoMapper;
using MediatR;
using Shared.DataTransferObjects;
using Wallet.App.Transaction.Queries;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Exceptions;

namespace Wallet.App.Transaction.Handlers;

internal sealed class GetTransactionsHandler(ILoggerManager logger, IRepositoryManager repository, IMapper mapper) : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionReadDto>>
{
    public async Task<IEnumerable<TransactionReadDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug($"GetTransactionsHandler: Getting transactions for account with id {request.AccountId}");

        var accountId = request!.AccountId;
        var isAccountExists = repository.Account.AccountExists(accountId);
        if (!isAccountExists)
        {
            throw new AccountNotFoundException(accountId);
        }

        var transaction = await repository.Transaction.GetAllTransactionsAsync(request.TrackChanges, cancellationToken);
        return mapper.Map<IEnumerable<TransactionReadDto>>(transaction);
    }
}