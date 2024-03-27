namespace Wallet.Shared.DataTransferObjects;

public sealed class AccountReadDto
{
    public Guid Id { get; set; }
    public string? AccountName { get; set; }
    public string? AccountType { get; set; }
    public decimal Balance { get; set; }
    public string? Currency { get; set; }
    public int TelegramUserId { get; set; }
}