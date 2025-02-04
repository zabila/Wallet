using Application.Messaging;
using Domain.Transactions;
using MediatR;

namespace Application.Transactions.Create;

public sealed class CreateTransactionCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public Location Location { get; set; }
    public string Attachment { get; set; }
}
