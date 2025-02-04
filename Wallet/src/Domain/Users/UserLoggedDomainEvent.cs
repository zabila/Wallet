using SharedKernel.Abstractions;

namespace Domain.Users;
public sealed record UserLoggedDomainEvent(Guid UserId) : IDomainEvent;
