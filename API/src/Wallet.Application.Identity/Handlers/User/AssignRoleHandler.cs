using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Commands.User;
using Wallet.Domain.Entities.Exceptions;
using Wallet.Domain.Entities.Model;

namespace Wallet.Application.Identity.Handlers.User;

public class AssignRoleHandler(UserManager<WalletIdentityUser> userManager, RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignRoleCommand> {
    public async Task Handle(AssignRoleCommand request, CancellationToken cancellationToken) {
        var assignRoleDto = request.AssignRoleDto;
        var email = assignRoleDto.Email!;
        var user = await userManager.FindByEmailAsync(email.ToLower());
        UserNotFoundException.ThrowIfNull(user, email);

        var roleExists = await roleManager.RoleExistsAsync(assignRoleDto.Role);
        if (!roleExists) {
            throw new RoleNotFoundException(assignRoleDto.Role);
        }

        var result = await userManager.AddToRoleAsync(user!, assignRoleDto.Role);
    }
}