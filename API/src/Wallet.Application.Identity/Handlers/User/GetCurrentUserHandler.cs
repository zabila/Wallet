using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Queries;
using Wallet.Domain.Entities.Model;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Identity.Handlers.User;

internal sealed class GetCurrentUserHandler(UserManager<WalletIdentityUser> userManager)
    : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    public async Task<CurrentUserDto> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request.Username, nameof(request.Username));
        WalletIdentityUser? user = await userManager.FindByNameAsync(request.Username);
        ArgumentNullException.ThrowIfNull(user, request.Username);

        IList<string> roles = await userManager.GetRolesAsync(user);
        ArgumentNullException.ThrowIfNull(roles, nameof(roles));

        return Guid.TryParse(user.Id, out Guid userId)
            ? new CurrentUserDto
            {
                Id = userId,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                TelegramUserName = user.TelegramUsername,
                TelegramUserId = user.TelegramUserId,
                IsEmailConfirmed = user.EmailConfirmed,
                IsPhoneNumberConfirmed = user.PhoneNumberConfirmed
            }
            : throw new InvalidCastException($"Cannot convert user.Id '{user.Id}' to Guid.");
    }
}
