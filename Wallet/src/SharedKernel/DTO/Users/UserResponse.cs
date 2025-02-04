namespace SharedKernel.DTO.Users;
public class UserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Guid TelegramId { get; set; }
    public Guid AccountId { get; set; }
    public string Localization { get; set; }
}
