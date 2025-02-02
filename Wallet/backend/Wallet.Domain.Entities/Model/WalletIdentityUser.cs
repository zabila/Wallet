using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Wallet.Domain.Entities.Model;

public class WalletIdentityUser : IdentityUser {
    [Required] public string? FirstName { get; set; }
    [Required] public string? LastName { get; set; }
    public string? TelegramUsername { get; set; }
    public int TelegramUserId { get; set; }

    public string? Localization { get; set; } = "en";
}