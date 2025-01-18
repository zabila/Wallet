using Wallet.Services.Telegram.Models;

namespace Wallet.Services.Telegram.Contracts;

public interface ISessionManager {
    Task<UserSession> GetOrCreateSessionAsync(long charId);
    Task RemoveSessionAsync(long chatId);
}