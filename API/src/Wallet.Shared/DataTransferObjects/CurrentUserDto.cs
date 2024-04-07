
namespace Wallet.Shared.DataTransferObjects;

public sealed class CurrentUserDto {
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TelegramUserName { get; set; }
    public int TelegramUserId { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public IList<string>? Roles { get; set; }
}
