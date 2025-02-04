using Wallet.Services.Telegram.Resources;

namespace Wallet.Services.Telegram.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class LocalizedDescriptionAttribute(string resourceKey) : Attribute
{
    public string ResourceKey { get; } = resourceKey;

    public string GetLocalizedDescription()
    {
        return TelegramBot.ResourceManager.GetString(ResourceKey, TelegramBot.Culture) ?? ResourceKey;
    }
}