namespace API.Telegram.Models;

public class LoggenUser
{
    public Guid UserId { get; set; }
    public Guid AccoundId { get; set; }
    public long TelegramUserId { get; set; }
    public string Localization { get; set; }
}
