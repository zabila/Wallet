using API.Telegram.Resources;

namespace API.Telegram.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public sealed class LocalizedDescriptionAttribute(string resourceKey) : Attribute
{
    public string ResourceKey { get; } = resourceKey;

    public string GetLocalizedDescription()
    {
        return TelegramBot.ResourceManager.GetString(ResourceKey, TelegramBot.Culture) ?? ResourceKey;
    }
}
