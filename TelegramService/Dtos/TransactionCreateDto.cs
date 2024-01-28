using System.ComponentModel.DataAnnotations;

namespace TelegramService.Dtos;

public class TransactionCreateDto
{
    public string? Name { get; set; }

    [Required(ErrorMessage = "Amount is a required field.")]
    public double? Amount { get; set; }

    [Required(ErrorMessage = "Date is a required field.")]
    public DateTime? Date { get; set; }

    [Required(ErrorMessage = "Type is a required field.")]
    public string? Type { get; set; }

    [Required(ErrorMessage = "Category is a required field.")]
    public string? Category { get; set; }

    public string? Description { get; set; }
}