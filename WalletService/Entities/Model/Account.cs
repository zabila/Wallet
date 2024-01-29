using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Model;

public sealed class Account
{
    [Key] public int AccountId { get; init; }

    [Required] [MaxLength(100)] public string? AccountName { get; init; }

    [Required] public DateTime CreatedAt { get; init; }
    [Required] public DateTime UpdatedAt { get; }

    [MaxLength(50)] public string? AccountType { get; init; }
    [Column(TypeName = "decimal(18, 2)")] public decimal Balance { get; init; }
}