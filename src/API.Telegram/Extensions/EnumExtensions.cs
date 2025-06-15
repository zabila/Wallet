using System.Reflection;
using API.Telegram.Attributes;

namespace API.Telegram.Extensions;

public static class EnumExtensions
{
    public static string GetLocalizedDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<LocalizedDescriptionAttribute>();
        return attribute?.GetLocalizedDescription() ?? value.ToString();
    }
}
