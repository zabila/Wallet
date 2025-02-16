using Domain.Users;
using MediatR;

namespace Application.Users.Register;

internal sealed class UserLoggedDomainEventHamdler : INotificationHandler<UserLoggedDomainEvent>
{
    public Task Handle(UserLoggedDomainEvent notification, CancellationToken cancellationToken)
    {
        // NOTE: Send an email verification link, etc.
        return Task.CompletedTask;
    }
}
