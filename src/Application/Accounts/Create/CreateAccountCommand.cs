using Application.Messaging;
using MediatR;

namespace Application.Accounts.Create;

public sealed class CreateAccountCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public string AccountName { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
}
