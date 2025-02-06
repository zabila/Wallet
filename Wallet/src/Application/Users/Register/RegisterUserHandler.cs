using Application.Authetication.Login;
using Application.Data;
using Application.Messaging;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.DTO.Login;

namespace Application.Users.Register;

internal sealed class RegisterUserHandler(IRepositoryManager repository, UserManager<User> userManager, ISender sender) : ICommandHandler<RegisterUserCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {

        if (await repository.Users.FindByCondition(user => user.Email == command.Email).AnyAsync(cancellationToken))
        {
            return Result.Failure<TokenResponse>(UserErrors.EmailNotUnique);
        }

        var user = new User
        {
            Email = command.Email,
            UserName = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Localization = command.Localization,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            return Result.Failure<TokenResponse>(UserErrors.OnReginsterUser(result.ToString()));
        }

        if (command.TelegramUserId.HasValue && !string.IsNullOrEmpty(command.TelegramUsername))
        {
            var telegramUser = new TelegramUser
            {
                Id = Guid.NewGuid(),
                TelegramUserId = command.TelegramUserId.Value,
                TelegramUsername = command.TelegramUsername,
                UserId = user.Id,
                User = user
            };
            await repository.TelegramUsers.CreateAsync(telegramUser, cancellationToken);

            user.TelegramUser = telegramUser;
            user.TelegramId = telegramUser.Id;
            repository.Users.Update(user);
        }

        await repository.SaveChangesAsync(cancellationToken);

        var tokenResponse = await sender.Send(new LoginCommand(command.Email, command.Password), cancellationToken);
        return tokenResponse;
    }
}
