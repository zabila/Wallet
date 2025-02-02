using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Queries;
using Wallet.Domain.Entities.Model;
using Wallet.Shared.DataTransferObjects;
using Wallet.Shared.Extensions;

namespace Wallet.Application.Identity.Handlers.User;

internal sealed class GetCurrentUserHandler(UserManager<WalletIdentityUser> userManager) : IRequestHandler<GetCurrentUserQuery, CurrentUserDto> {
    public async Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken) {

        var user = await userManager.FindByNameAsync(request.Username.EnsureExists());
        var currentUser = user.EnsureExists();
        IList<string> roles = await userManager.GetRolesAsync(currentUser);

        return Guid.TryParse(currentUser.Id, out Guid userId)
            ? new CurrentUserDto {
                Id = userId,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Roles = roles,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhoneNumber = currentUser.PhoneNumber,
                TelegramUserName = currentUser.TelegramUsername,
                TelegramUserId = currentUser.TelegramUserId,
                IsEmailConfirmed = currentUser.EmailConfirmed,
                IsPhoneNumberConfirmed = currentUser.PhoneNumberConfirmed
            }
            : throw new InvalidCastException($"Cannot convert user.Id '{currentUser.Id}' to Guid.");
    }
}
