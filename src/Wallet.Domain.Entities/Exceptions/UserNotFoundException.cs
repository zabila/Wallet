using Wallet.Domain.Entities.Model;

namespace Wallet.Domain.Entities.Exceptions;

public class UserNotFoundException(string email) : NotFoundException($"The user with email: {email} doesn't exist.")
{
    public static void ThrowIfNull(object? obj, string email)
    {
        if (obj is null)
        {
            throw new UserNotFoundException(email);
        }
    }
}