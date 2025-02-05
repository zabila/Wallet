using Application.Data;
using Application.Messaging;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.Exceptions;
using SharedKernel.Extensions;

namespace Application.Users.Update;

internal sealed class UpdateUserHandler(IRepositoryManager repository) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await repository.Users.FindByCondition(user => user.Id == command.UserId, true).SingleOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        user.FirstName = command.FirstName;
        user.LastName = command.LastName;

        var telegramUser = await repository.TelegramUsers.
            FindByCondition(telegramUser => telegramUser.Id == user.TelegramId, true).SingleOrDefaultAsync(cancellationToken);

        if (telegramUser is null)
        {
            var newTelegramUser = new TelegramUser
            {
                TelegramUserId = command.TelegramUserId,
                TelegramUsername = command.TelegramUsername,
                UserId = user.Id
            };
            await repository.TelegramUsers.CreateAsync(newTelegramUser, cancellationToken);
        }
        else
        {
            telegramUser.TelegramUsername = command.TelegramUsername;
            telegramUser.TelegramUserId = command.TelegramUserId;
        }

        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
