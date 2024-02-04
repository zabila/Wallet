using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public class LoginUserDto
{
    [Required] public string? Email { get; set; }
    [Required] public string? Password { get; set; }
}