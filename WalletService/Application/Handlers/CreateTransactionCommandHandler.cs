using Application.Commands;
using AutoMapper;
using Contracts;
using Entities.Model;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Handlers;

internal sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public CreateTransactionCommandHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactionDto = request?.TransactionForCreationDto;
        var transaction = _mapper.Map<Transaction>(transactionDto);

        _repository.Transaction.CreateTransaction(transaction);
        await _repository.SaveAsync();

        var transactionToReturn = _mapper.Map<TransactionDto>(transaction);
        return transactionToReturn;
    }
}