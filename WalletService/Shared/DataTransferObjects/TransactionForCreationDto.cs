using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record TransactionForCreationDto
{
    public string? Name { get; init; }
    public double? Amount { get; init; }
    public string? Type { get; init; }
    public string? Category { get; init; }
    public string? Description { get; init; }
    public int TelegramUserId { get; init; }
}