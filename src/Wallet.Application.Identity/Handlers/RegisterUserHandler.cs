using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Commands;
using Wallet.Domain.Entities.Model;

namespace Wallet.Application.Identity.Handlers;

internal sealed class RegisterUserHandler(UserManager<User> userManager, IMapper mapper) : IRequestHandler<RegisterUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var registerUserDto = request.RegisterUserDto;
        var user = mapper.Map(registerUserDto, new User());
        var result = await userManager.CreateAsync(user, registerUserDto.Password!);
        return result;
    }
}