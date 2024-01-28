using Entities.Model;

namespace Contracts;

public interface IUserRepository
{
    void CreateUser(User user);
    void DeleteUser(User user);
    void UpdateUser(User user);

    Task<User?> GetUserByTelegramUserIdAsync(int? telegramUserId, bool trackChanges);

    Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges);
}