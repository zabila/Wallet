namespace TelegramService.Dtos;

public class TransactionPublishedDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Event { get; set; }

    public decimal Amount { get; set; }
    public string? Type { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public int TelegramUserId { get; set; }
}