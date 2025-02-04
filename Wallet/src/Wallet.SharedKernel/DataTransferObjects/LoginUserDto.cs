using System.ComponentModel.DataAnnotations;

namespace Wallet.SharedKernel.DataTransferObjects;

public class LoginUserDto
{
    [Required] public string? Email { get; set; }
    [Required] public string? Password { get; set; }
}