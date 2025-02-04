using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Queries;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Model;
using Wallet.Shared.DataTransferObjects;
using Wallet.Shared.Extensions;

namespace Wallet.Application.Identity.Handlers.User;

internal sealed class GetCurrentUserByTelegramHandler(IRepositoryManager repository) : IRequestHandler<GetCurrentUserByTelegramIdQuery, CurrentUserDto>
{
    public async Task<CurrentUserDto> Handle(GetCurrentUserByTelegramIdQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await repository.WalletIdentityUsers.FindUserByTelegramUserIdAsync(request.TelegramId, false, cancellationToken);
        currentUser = currentUser.EnsureExists();

        return Guid.TryParse(currentUser.Id, out Guid userId)
            ? new CurrentUserDto
            {
                Id = userId,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Roles = [],
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhoneNumber = currentUser.PhoneNumber,
                TelegramUserName = currentUser.TelegramUsername,
                TelegramUserId = currentUser.TelegramUserId,
                IsEmailConfirmed = currentUser.EmailConfirmed,
                IsPhoneNumberConfirmed = currentUser.PhoneNumberConfirmed,
                Localization = currentUser.Localization
            }
            : throw new InvalidCastException($"Cannot convert user.Id '{currentUser.Id}' to Guid.");
    }
}