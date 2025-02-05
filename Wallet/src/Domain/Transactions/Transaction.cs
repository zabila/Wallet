using SharedKernel;

namespace Domain.Transactions;

public sealed class Transaction : Entity
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Location Location { get; set; }
    public string Attachment { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
}
