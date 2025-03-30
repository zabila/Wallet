namespace SharedKernel.DTO.Users;

public class UpdateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TelegramUserId { get; set; }
    public string TelegramUsername { get; set; }
    public string Localization { get; set; }
}
