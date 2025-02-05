using SharedKernel;

namespace Domain.Accounts;

public sealed class Account : Entity
{
    public Guid Id { get; set; }
    public string AccountName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
}
