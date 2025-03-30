using Domain.Transactions;

namespace MessageBus.Events;

public class CreateTransactionEvent : GenericEvent
{
    public CreateTransactionEvent()
    {
        Event = nameof(CreateTransactionEvent);
    }

    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public Location? Location { get; set; }
    public string Attachment { get; set; }
}
