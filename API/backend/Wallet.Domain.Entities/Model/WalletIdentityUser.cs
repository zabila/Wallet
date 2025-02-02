using Microsoft.AspNetCore.Identity;

namespace Wallet.Domain.Entities.Model;

public class WalletIdentityUser : IdentityUser {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? TelegramUsername { get; set; }
    public int TelegramUserId { get; set; }
}