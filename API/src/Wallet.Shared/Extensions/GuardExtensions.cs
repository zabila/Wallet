namespace Wallet.Shared.Extensions;

public static class GuardExtensions {
    public static T EnsureExists<T>(this T? obj) where T : class? {
        ArgumentNullException.ThrowIfNull(obj);
        return obj;
    }

    public static string EnsureExists(this string? obj) {
        ArgumentException.ThrowIfNullOrEmpty(obj);
        return obj;
    }
}