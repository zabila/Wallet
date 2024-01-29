using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public sealed class AccountCreateDto
{
    [Required] [MaxLength(100)] public string? AccountName { get; set; }
    [MaxLength(50)] public string? AccountType { get; set; }
    [Range(0, (double)decimal.MaxValue)] public decimal Balance { get; set; }
}