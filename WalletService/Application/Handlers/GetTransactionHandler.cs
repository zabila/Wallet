using Application.Queries.Transaction;
using AutoMapper;
using Contracts;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Handlers;

internal sealed class GetTransactionHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IRequestHandler<GetTransactionQuery, TransactionDto>
{
    public Task<TransactionDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug($"GetTransactionHandler: Getting transaction with id {request.Id}");

        var transactionId = request.Id;
        var transaction = repository.Transaction.GetTransaction(transactionId, request.TrackChanges);
        var transactionDto = mapper.Map<TransactionDto>(transaction);
        return Task.FromResult(transactionDto);
    }
}