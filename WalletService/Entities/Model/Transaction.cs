namespace Entities.Model;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public sealed class Transaction
{
    [Key] [Column("TransactionId")] public Guid Id { get; init; }
    [Required] public DateTime Date { get; init; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; init; }

    [Required] [MaxLength(100)] public string? Description { get; init; }
    [Required] [MaxLength(50)] public string? Category { get; init; }
    [Required] [MaxLength(20)] public string? Type { get; init; }

    [Required] public DateTime CreatedAt { get; init; }
    [Required] public DateTime UpdatedAt { get; }

    [MaxLength(10)] public string? Currency { get; init; }
    [MaxLength(100)] public string? Location { get; init; }
    [MaxLength(50)] public string? ReferenceNumber { get; init; }
    public string? Tags { get; init; }
    public string? Attachment { get; init; }
    [Required] public int AccountID { get; init; }
    [ForeignKey("AccountId")] public Account? Account { get; init; }
}