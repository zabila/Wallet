namespace Shared.DataTransferObjects;

public sealed class AccountReadDto
{
    public Guid Id { get; set; }
    public string? AccountName { get; set; }
    public string? AccountType { get; set; }
    public decimal Balance { get; set; }
}