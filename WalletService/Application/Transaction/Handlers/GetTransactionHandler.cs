using Application.Transaction.Queries;
using AutoMapper;
using Contracts;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Transaction.Handlers;

internal sealed class GetTransactionHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IRequestHandler<GetTransactionQuery, TransactionReadDto>
{
    public Task<TransactionReadDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug($"GetTransactionHandler: Getting transaction with id {request.Id}");

        var transactionId = request.Id;
        var transaction = repository.Transaction.GetTransaction(transactionId, request.TrackChanges);
        var transactionDto = mapper.Map<TransactionReadDto>(transaction);
        return Task.FromResult(transactionDto);
    }
}