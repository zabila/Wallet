using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Model;

public sealed class Account
{
    [Key] public int AccountId { get; set; }

    [Required] [MaxLength(100)] public string? AccountName { get; set; }

    [Required] public DateTime CreatedAt { get; set; }
    [Required] public DateTime UpdatedAt { get; set; }

    [MaxLength(50)] public string? AccountType { get; set; }
    [Column(TypeName = "decimal(18, 2)")] public decimal Balance { get; set; }
}