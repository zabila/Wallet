namespace Wallet.Domain.Entities.Model;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public sealed class Transaction
{
    [Key] [Column("TransactionId")] public Guid Id { get; set; }
    [Required] public DateTime Date { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Required] [MaxLength(100)] public string? Description { get; set; }
    [Required] [MaxLength(50)] public string? Category { get; set; }
    [Required] [MaxLength(20)] public string? Type { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required] public DateTime UpdatedAt { get; set; }

    [MaxLength(10)] public string? Currency { get; set; }
    [MaxLength(100)] public string? Location { get; set; }
    public string? Tags { get; set; }
    public string? Attachment { get; set; }
    [Required] public Guid AccountId { get; set; }
}