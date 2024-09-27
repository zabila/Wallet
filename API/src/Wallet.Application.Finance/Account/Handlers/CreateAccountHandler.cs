using AutoMapper;
using MediatR;
using Wallet.Application.Finance.Account.Commands;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Exceptions;
using Wallet.Domain.Entities.Model;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Finance.Account.Handlers;

internal sealed class CreateAccountHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
    : IRequestHandler<CreateAccountCommand, AccountReadDto> {
    public async Task<AccountReadDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken) {
        logger.LogDebug("CreateAccountHandler: Creating account");

        AccountCreateDto? accountDto = request?.AccountCreateDto;
        await CheckIfAccountAlreadyExistsAsync(accountDto!);

        Domain.Entities.Model.Account? account = mapper.Map<Wallet.Domain.Entities.Model.Account>(accountDto);
        repository.Account.CreateAccount(account);

        AccountTelegram accountTelegram = new AccountTelegram {
            AccountId = account.Id,
            TelegramUserId = accountDto!.TelegramUserId
        };

        SaveAccountTelegramIfNotExists(accountTelegram);

        await repository.SaveAsync(cancellationToken);
        AccountReadDto? accountToReturn = mapper.Map<AccountReadDto>(account);
        accountToReturn.TelegramUserId = accountDto.TelegramUserId;
        return accountToReturn;
    }

    private async Task CheckIfAccountAlreadyExistsAsync(AccountCreateDto accountDto) {
        Domain.Entities.Model.Account? existingAccount =
            await repository.Account.GetAccountByNameAsync(accountDto!.AccountName!);

        if (existingAccount != null) {
            throw new AccountAlreadyExistsBadRequestException(accountDto.AccountName!);
        }
    }

    private void SaveAccountTelegramIfNotExists(AccountTelegram accountTelegram) {
        bool isExistAccountTelegram = repository.AccountTelegrams.AccountTelegramExists(accountTelegram);
        if (!isExistAccountTelegram) {
            repository.AccountTelegrams.CreateAccountTelegram(accountTelegram);
        }
    }
}