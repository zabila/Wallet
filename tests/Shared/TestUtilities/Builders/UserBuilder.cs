using Domain.Users;

namespace TestUtilities.Builders;

public class UserBuilder
{
    private Guid _id;
    private string _email = "test@example.com";
    private string _firstName = "Test";
    private string _lastName = "User";
    private DateTime _createdAt = DateTime.UtcNow;
    private DateTime _updatedAt = DateTime.UtcNow;
    private string _localization = "en-US";
    private long? _telegramUserId = 12345;
    private string? _telegramUsername = "testuser";

    public static UserBuilder Create() => new();

    public UserBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public UserBuilder WithLastName(string lastName)
    {
        _lastName = lastName;
        return this;
    }

    public UserBuilder WithTelegramUserId(long? telegramUserId)
    {
        _telegramUserId = telegramUserId;
        return this;
    }

    public UserBuilder WithTelegramUsername(string? telegramUsername)
    {
        _telegramUsername = telegramUsername;
        return this;
    }

    public UserBuilder WithLocalization(string localization)
    {
        _localization = localization;
        return this;
    }

    public UserBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public UserBuilder WithUpdatedAt(DateTime updatedAt)
    {
        _updatedAt = updatedAt;
        return this;
    }

    public User Build()
    {
        var user = new User
        {
            Id = _id == Guid.Empty ? Guid.NewGuid() : _id,
            Email = _email,
            UserName = _email,
            FirstName = _firstName,
            LastName = _lastName,
            CreatedAt = _createdAt,
            UpdatedAt = _updatedAt,
            Localization = _localization
        };

        if (_telegramUserId.HasValue && !string.IsNullOrEmpty(_telegramUsername))
        {
            user.TelegramUser = new TelegramUser
            {
                Id = Guid.NewGuid(),
                TelegramUserId = _telegramUserId.Value,
                TelegramUsername = _telegramUsername,
                UserId = user.Id,
                User = user,
                CreatedAt = _createdAt,
                UpdatedAt = _updatedAt
            };
            user.TelegramId = user.TelegramUser.Id;
        }

        return user;
    }
}