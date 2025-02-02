using System.Reflection;
using Wallet.Services.Telegram.Attributes;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.Extensions;

public static class EnumExtensions {
    public static string GetLocalizedDescription(this Enum value) {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<LocalizedDescriptionAttribute>();
        return attribute?.GetLocalizedDescription() ?? value.ToString();
    }
}