using Application.Account.Queries;
using AutoMapper;
using MediatR;
using Wallet.Shared.DataTransferObjects;
using Wallet.Domain.Contracts;

namespace Application.Account.Handlers;

internal sealed class GetAccountByTelegramUserIdHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager logger) : IRequestHandler<GetAccountByTelegramUserIdQuery, AccountReadDto>
{
    public async Task<AccountReadDto> Handle(GetAccountByTelegramUserIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug("GetAccountByTelegramUserIdHandler: Getting account by telegram user id");

        var accountGuid = await repository.AccountTelegrams.GetAccountIdByTelegramUserIdAsync(request.TelegramUserId, false, cancellationToken);
        var account = await repository.Account.GetAccountAsync(accountGuid, false, cancellationToken);
        return mapper.Map<AccountReadDto>(account);
    }
}