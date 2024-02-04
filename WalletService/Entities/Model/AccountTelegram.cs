using System.ComponentModel.DataAnnotations;

namespace Entities.Model;

public sealed class AccountTelegram
{
    [Key] public int TelegramUserId { get; set; }
    public Guid AccountId { get; set; }
}