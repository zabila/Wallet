using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Commands.User;
using Wallet.Domain.Entities.Exceptions;
using Wallet.Domain.Entities.Model;

namespace Wallet.Application.Identity.Handlers.User;

internal sealed class UpdateUserHandler(UserManager<WalletIdentityUser> userManager, IMapper mapper) : IRequestHandler<UpdateUserCommand, IdentityResult> {
    public async Task<IdentityResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
        var updateUserDto = request.UpdateUserDto;
        var updateUser = mapper.Map<WalletIdentityUser>(updateUserDto);

        var email = updateUser.Email!;
        var user = await userManager.FindByEmailAsync(email);
        UserNotFoundException.ThrowIfNull(user, email);

        var result = await userManager.UpdateAsync(user!);
        return result;
    }
}