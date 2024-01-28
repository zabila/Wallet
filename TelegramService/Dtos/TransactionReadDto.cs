namespace TelegramService.Dtos;

public class TransactionReadDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? Amount { get; set; }
    public DateTime? Date { get; set; }
    public string? Type { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
}