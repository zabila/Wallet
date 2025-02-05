using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Domain.Users;

public sealed class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Guid TelegramId { get; set; }
    public Guid AccountId { get; set; }
    public string Localization { get; set; }
}
