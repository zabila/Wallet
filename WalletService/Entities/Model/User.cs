using Microsoft.AspNetCore.Identity;

namespace Entities.Model;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? TelegramUsername { get; set; }
    public int TelegramUserId { get; set; }
}