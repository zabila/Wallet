namespace Wallet.Services.Telegram.Dtos;

public record TransactionPublishedDto {
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? Event { get; init; } = "TransactionTelegramPublished";
    public DateTime Date { get; init; } = DateTime.UtcNow;
    public decimal Amount { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
    public string? Type { get; init; }
    public string? Currency { get; init; } = "UAH";
    public string? Location { get; init; }
    public string? Tags { get; init; } = "Telegram";
    public string? Attachment { get; init; } = "Telegram";
    public int TelegramUserId { get; init; }
}