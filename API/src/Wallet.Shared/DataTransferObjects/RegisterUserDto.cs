using System.ComponentModel.DataAnnotations;

namespace Wallet.Shared.DataTransferObjects;

public class RegisterUserDto
{
    [Required] public string? Email { get; set; }
    [Required] public string? Password { get; set; }
    [Required] public string? FirstName { get; set; }
    [Required] public string? LastName { get; set; }
    public int TelegramUserId { get; set; }
    public string? TelegramUsername { get; set; }
}