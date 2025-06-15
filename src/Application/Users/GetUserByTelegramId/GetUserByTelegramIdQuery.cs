using Application.Messaging;
using SharedKernel.DTO.Users;

namespace Application.Users.GetUserByTelegramId;

public sealed record GetUserByTelegramIdQuery(long TelegramId) : IQuery<UserResponse>;
