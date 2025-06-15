using System.ComponentModel.DataAnnotations;
namespace SharedKernel.DTO.Transactions;

public class TransactionsRequest
{
    public decimal Amount { get; set; }
    [Required, MinLength(1)]
    public string Category { get; set; }
    [Required, MinLength(1)]
    public string Type { get; set; }
    [Required]
    public Location Location { get; set; }
    public string Attachment { get; set; }
}
