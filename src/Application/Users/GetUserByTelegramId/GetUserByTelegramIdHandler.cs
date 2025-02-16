using Application.Data;
using Application.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.DTO.Users;

namespace Application.Users.GetUserByTelegramId;
internal sealed class GetUserByTelegramIdHandler(IRepositoryManager repository) : IQueryHandler<GetUserByTelegramIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByTelegramIdQuery request, CancellationToken cancellationToken)
    {
        var telegramUser = await repository.TelegramUsers.FindByCondition(Users => Users.TelegramUserId == request.TelegramId).FirstOrDefaultAsync(cancellationToken);
        if (telegramUser == null)
        {
            return Result.Failure<UserResponse>(UserErrors.TelegreamUserNotFound(request.TelegramId));
        }

        var user = telegramUser.User;
        if (null == user)
        {
            return Result.Failure<UserResponse>(UserErrors.NotLinkedWithUser);
        }

        var userResponse = new UserResponse
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Localization = user.Localization,
            AccountId = user.Id,
            TelegramId = user.Id
        };

        return Result.Success(userResponse);
    }
}
