using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record TransactionDto
{
    [Required(ErrorMessage = "Id is required")]
    public Guid Id { get; init; }

    public string? Name { get; init; }
    public double? Amount { get; init; }
    public DateTime? Date { get; init; }
    public string? Type { get; init; }
    public string? Category { get; init; }
    public string? Description { get; init; }
    public int TelegramUserId { get; init; }
}