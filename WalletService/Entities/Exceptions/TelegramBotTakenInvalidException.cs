namespace Entities.Exceptions;

public class TelegramBotTokenInvalidException : Exception
{
    public TelegramBotTokenInvalidException()
        : base("Telegram bot token is invalid")
    {
    }

    public TelegramBotTokenInvalidException(string message)
        : base(message)
    {
    }

    public TelegramBotTokenInvalidException(string message, Exception inner)
        : base(message, inner)
    {
    }

    [Obsolete("Obsolete")]
    protected TelegramBotTokenInvalidException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }
}