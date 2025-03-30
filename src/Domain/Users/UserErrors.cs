using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "The user with the specified email was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static readonly Error CannotUpdateUser = Error.Problem(
        "Users.CannotUpdateUser",
        "The user cannot be updated");

    public static Error AnyTelegreamUserNotFound => Error.NotFound(
        "TelegreamUser.AnyNotFound",
        "The user was not found");

    public static Error NotLinkedWithUser => Error.NotFound(
        "TelegreamUser.NotLinkedWithUser",
        "The user is not linked with any user");

    public static Error NotFound(Guid userId)
    {
        return Error.NotFound(
            "Users.NotFound",
            $"The user with the Id = '{userId}' was not found");
    }

    public static Error TelegreamUserNotFound(long userId)
    {
        return Error.NotFound(
            "TelegreamUser.NotFound",
            $"The user with the Id = '{userId}' was not found");
    }

    public static Error Unauthorized()
    {
        return Error.Failure(
            "Users.Unauthorized",
            "You are not authorized to perform this action.");
    }

    public static Error OnReginsterUser(string reason)
    {
        return Error.Problem(
            "Users.RegisterUser",
            $"The user cannot be added, Reason: {reason}");
    }
}
