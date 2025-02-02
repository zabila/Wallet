namespace Wallet.Shared.Extensions;

public static class GuardExtensions {
    public static T EnsureExists<T>(this T? obj) where T : class? {
        var result = obj ?? throw new ArgumentNullException(typeof(T).Name);
        return result;
    }

    public static string EnsureExists(this string? obj) {
        ArgumentException.ThrowIfNullOrEmpty(obj);
        return obj;
    }
}