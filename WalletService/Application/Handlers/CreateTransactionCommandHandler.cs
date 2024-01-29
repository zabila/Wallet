using Application.Commands;
using AutoMapper;
using Contracts;
using Entities.Model;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Handlers;

internal sealed class CreateTransactionCommandHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger) : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly ILoggerManager _logger = logger;

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionDto = request?.TransactionForCreationDto;
        var transaction = mapper.Map<Transaction>(transactionDto);

        repository.Transaction.CreateTransaction(transaction);
        await repository.SaveAsync();

        var transactionToReturn = mapper.Map<TransactionDto>(transaction);
        return transactionToReturn;
    }
}