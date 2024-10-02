using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Commands.Authentication;
using Wallet.Domain.Contracts;
using Wallet.Domain.Entities.Model;
using Wallet.Shared.Extensions;

namespace Wallet.Application.Identity.Handlers.Authentication;

internal sealed class LoginHandler(UserManager<WalletIdentityUser> userManager, SignInManager<WalletIdentityUser> signInManager, ILoggerManager logger) : IRequestHandler<LoginCommand, SignInResult> {
    public async Task<SignInResult> Handle(LoginCommand request, CancellationToken cancellationToken) {

        var userForAuthenticationDto = request.UserForAuthenticationDto.EnsureExists();

        var user = await userManager.FindByEmailAsync(userForAuthenticationDto.UserName.EnsureExists());
        if (user is null) {
            logger.LogError($"User with email {userForAuthenticationDto.UserName} not found.");
            return SignInResult.Failed;
        }
        var result = await signInManager.CheckPasswordSignInAsync(user, userForAuthenticationDto.Password, false);
        return result;
    }
}