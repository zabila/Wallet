namespace TelegramService.Dtos;

public class TransactionPublishedDto
{
    public Guid Id { get; set; }
    public string? Event { get; set; } = "TransactionTelegramPublished";
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Type { get; set; }
    public string? Currency { get; set; }
    public string? Location { get; set; }
    public string? Tags { get; set; }
    public string? Attachment { get; set; }
    public int TelegramUserId { get; set; }
}