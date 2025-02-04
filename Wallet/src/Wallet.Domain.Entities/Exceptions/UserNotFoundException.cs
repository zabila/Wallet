namespace Wallet.Domain.Entities.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string email) : base($"The user with email: {email} doesn't exist.")
    {
    }

    public UserNotFoundException() : base("The user doesn't exist.")
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static void ThrowIfNull(object? obj, string email)
    {
        if (obj is null)
        {
            throw new UserNotFoundException(email);
        }
    }
}
