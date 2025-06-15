using System.ComponentModel.DataAnnotations;
namespace SharedKernel.DTO.Accounts;

public class AccountCreateRequest
{
    [Required, MinLength(1)]
    public string AccountName { get; set; }
    [Required, MinLength(1)]
    public string AccountType { get; set; }
    public decimal Balance { get; set; }
    [Required, MinLength(1)]
    public string Currency { get; set; }
}
