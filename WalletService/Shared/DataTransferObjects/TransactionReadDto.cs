namespace Shared.DataTransferObjects;

public class TransactionReadDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Currency { get; set; }
    public string? Location { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Tags { get; set; }
    public string? Attachment { get; set; }
    public int? AccountID { get; set; }
}