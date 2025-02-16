using Domain.Accounts;
using Domain.Transactions;
using Microsoft.AspNetCore.Identity;

namespace Domain.Users;

public sealed class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Localization { get; set; }

    public List<Transaction> Transactions { get; set; } = new();

    public Guid? TelegramId { get; set; }
    public TelegramUser? TelegramUser { get; set; }

    public Guid? AccountId { get; set; }
    public Account? Account { get; set; }
}
