using Application.Account.Commands;
using AutoMapper;
using MediatR;
using Wallet.Shared.DataTransferObjects;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Exceptions;
using Wallet.Domain.Entities.Model;

namespace Application.Account.Handlers;

internal sealed class CreateAccountHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger) : IRequestHandler<CreateAccountCommand, AccountReadDto>
{
    public async Task<AccountReadDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        logger.LogDebug("CreateAccountHandler: Creating account");

        var accountDto = request?.AccountCreateDto;
        await CheckIfAccountAlreadyExists(accountDto!);

        var account = mapper.Map<Wallet.Domain.Entities.Model.Account>(accountDto);
        repository.Account.CreateAccount(account);

        var accountTelegram = new AccountTelegram
        {
            AccountId = account.Id,
            TelegramUserId = accountDto!.TelegramUserId
        };

        SaveAccountTelegramIfNotExists(accountTelegram);

        await repository.SaveAsync(cancellationToken);
        var accountToReturn = mapper.Map<AccountReadDto>(account);
        accountToReturn.TelegramUserId = accountDto.TelegramUserId;
        return accountToReturn;
    }

    private async Task CheckIfAccountAlreadyExists(AccountCreateDto accountDto)
    {
        var existingAccount = await repository.Account.GetAccountByNameAsync(accountDto!.AccountName!);
        if (existingAccount != null)
        {
            throw new AccountAlreadyExistsBadRequestException(accountDto.AccountName!);
        }
    }

    private void SaveAccountTelegramIfNotExists(AccountTelegram accountTelegram)
    {
        var isExistAccountTelegram = repository.AccountTelegrams.AccountTelegramExists(accountTelegram);
        if (!isExistAccountTelegram)
        {
            repository.AccountTelegrams.CreateAccountTelegram(accountTelegram);
        }
    }
}