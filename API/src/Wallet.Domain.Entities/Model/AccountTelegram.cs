using System.ComponentModel.DataAnnotations;

namespace Wallet.Domain.Entities.Model;

public sealed class AccountTelegram {
    [Key] public int TelegramUserId { get; set; }
    public Guid AccountId { get; set; }
}