using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Commands.User;
using Wallet.Domain.Entities.Model;

namespace Wallet.Application.Identity.Handlers.User;

internal sealed class RegisterUserHandler(UserManager<WalletIdentityUser> userManager, IMapper mapper) : IRequestHandler<RegisterUserCommand, IdentityResult> {
    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken) {
        var registerUserDto = request.RegisterUserDto;
        var user = mapper.Map(registerUserDto, new WalletIdentityUser());
        var result = await userManager.CreateAsync(user, registerUserDto.Password!);
        return result;
    }
}