using Application.Account.Commands;
using Entities.Model;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using MediatR;
using Shared.DataTransferObjects;

namespace Application.Account.Handlers;

internal sealed class CreateAccountHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger) : IRequestHandler<CreateAccountCommand, AccountReadDto>
{
    public async Task<AccountReadDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        logger.LogDebug($"CreateAccountHandler: Creating account");

        var accountDto = request?.AccountCreateDto;

        var existingAccount = await repository.Account.GetAccountByNameAsync(accountDto!.AccountName!);
        if (existingAccount != null)
        {
            throw new AlreadyExistsBadRequestException(nameof(AccountCreateDto.AccountName));
        }

        var account = mapper.Map<Entities.Model.Account>(accountDto);

        repository.Account.CreateAccount(account);
        await repository.SaveAsync(cancellationToken);

        var accountToReturn = mapper.Map<AccountReadDto>(account);
        return accountToReturn;
    }
}