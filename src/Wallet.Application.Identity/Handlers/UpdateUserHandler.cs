using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.Identity.Commands;
using Wallet.Domain.Entities.Exceptions;
using Wallet.Domain.Entities.Model;

namespace Wallet.Application.Identity.Handlers;

internal sealed class UpdateUserHandler(UserManager<User> userManager, IMapper mapper) : IRequestHandler<UpdateUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateUserDto = request.UpdateUserDto;
        var updateUser = mapper.Map<User>(updateUserDto);
        
        var email = updateUser.Email!;
        var user = await userManager.FindByEmailAsync(email);
        UserNotFoundException.ThrowIfNull(user, email);
        
        var result = await userManager.UpdateAsync(user!);
        return result;
    }
}