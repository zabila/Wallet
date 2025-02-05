using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");

    public static Error TelegreamUserNotFound(long userId) => Error.NotFound(
        "TelegreamUser.NotFound",
        $"The user with the Id = '{userId}' was not found");

    public static Error AnyTelegreamUserNotFound => Error.NotFound(
        "TelegreamUser.AnyNotFound",
        "The user was not found");

    public static Error Unauthorized() => Error.Failure(
        "Users.Unauthorized",
        "You are not authorized to perform this action.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "The user with the specified email was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static readonly Error CannotAddUser = Error.Problem(
        "Users.CannotAddUser",
        "The user cannot be added");

    public static readonly Error CannotUpdateUser = Error.Problem(
        "Users.CannotUpdateUser",
        "The user cannot be updated");
}
