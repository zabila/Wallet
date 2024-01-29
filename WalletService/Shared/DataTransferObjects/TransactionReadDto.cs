namespace Shared.DataTransferObjects;

public class TransactionReadDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Type { get; set; }
    public string? Currency { get; set; }
    public string? Location { get; set; }
    public string? Tags { get; set; }
    public string? Attachment { get; set; }
}