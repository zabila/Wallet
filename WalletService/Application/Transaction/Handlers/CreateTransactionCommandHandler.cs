using Application.Transaction.Commands;
using AutoMapper;
using Contracts;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Transaction.Handlers;

internal sealed class CreateTransactionCommandHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger) : IRequestHandler<CreateTransactionCommand, TransactionReadDto>
{
    private readonly ILoggerManager _logger = logger;

    public async Task<TransactionReadDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionDto = request?.TransactionForCreationDto;
        var transaction = mapper.Map<Entities.Model.Transaction>(transactionDto);

        repository.Transaction.CreateTransaction(transaction);
        await repository.SaveAsync(cancellationToken);

        var transactionToReturn = mapper.Map<TransactionReadDto>(transaction);
        return transactionToReturn;
    }
}