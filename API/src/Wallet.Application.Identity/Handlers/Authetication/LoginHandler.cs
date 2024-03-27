using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Commands.Authetication;
using Wallet.Domain.Entities.Model;

namespace Wallet.Application.Identity.Handlers.Authetication;

internal sealed class LoginHandler(UserManager<WalletIdentityUser> userManager, SignInManager<WalletIdentityUser> signInManager) : IRequestHandler<LoginCommand, SignInResult> {
    public async Task<SignInResult> Handle(LoginCommand request, CancellationToken cancellationToken) {
        var userForAuthenticationDto = request.UserForAuthenticationDto;
        var user = await userManager.FindByEmailAsync(userForAuthenticationDto.UserName!);
        if (user is null) {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, userForAuthenticationDto.Password, false);
        return result;
    }
}